using System.Text.Json;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Extensions.Logging;

namespace ExpensifyImporter.Library.Modules.Excel
{
    public class ExcelReader
    {
        private readonly ILogger<ExcelReader> _logger;

        public ExcelReader(ILogger<ExcelReader> logger)
        {
           _logger = logger;
        }


        public string ReadAsJson(string path)
        {
            var bookList = new List<List<string[]>>();
            try
            {
                //Lets open the existing excel file and read through its content . Open the excel using openxml sdk
                using var excelDocument = SpreadsheetDocument.Open(path, false);

                var documentBody = excelDocument?.WorkbookPart?.Workbook;
                
                var worksheetParts = documentBody?.WorkbookPart?.WorksheetParts.ToList();

                if (worksheetParts == null) return JsonSerializer.Serialize(bookList);
                
                foreach (var worksheetPart in worksheetParts)
                {
                    var sheet = worksheetPart.Worksheet;
                    var rows = sheet.Descendants<Row>().ToList();
                    var list = (from row in rows
                        let sharedStringTable = excelDocument?.WorkbookPart?.SharedStringTablePart?.SharedStringTable 
                        select row.Elements<Cell>().Select(cell => sharedStringTable != null ? GetExcelCellValue(cell, sharedStringTable) : null)
                            .ToArray())
                        .ToList();

                    bookList.Add(list);
                }

                return JsonSerializer.Serialize(bookList);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }


        private string? GetExcelCellValue(Cell cell, SharedStringTable sharedStringTable)
        {
            if (cell.CellValue == null) return cell.InnerText;
            if (cell?.DataType != null && cell.DataType == CellValues.SharedString)
            {
                if (!int.TryParse(cell.InnerText, out var id)) return null;
                var item = sharedStringTable.Elements<SharedStringItem>().ElementAt(id);
                if (item.Text != null)
                {
                    return item.Text.Text;
                }

                if (item.InnerText != null)
                {
                    return item.InnerText;
                }

                if (item.InnerXml != null)
                {
                    return item.InnerXml;
                }
            }
            else
            {
                return cell?.CellValue.Text;
            }
            return null;
        }
    }
}

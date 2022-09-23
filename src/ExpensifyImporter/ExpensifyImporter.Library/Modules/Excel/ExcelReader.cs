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
                using SpreadsheetDocument excelDocument = SpreadsheetDocument.Open(path, false);

                var documentBody = excelDocument?.WorkbookPart?.Workbook;
                var worksheetParts = documentBody?.WorkbookPart?.WorksheetParts;

                if (worksheetParts == null) return JsonSerializer.Serialize(bookList);
                foreach (var worksheetPart in worksheetParts)
                {
                    var sheet = worksheetPart.Worksheet;
                    var rows = sheet.Descendants<Row>().ToList();
                    var list = new List<string[]>();

                    foreach (var row in rows)
                    {
                        list.Add(row.Elements<Cell>().Select(cell =>
                        {
                            if (cell.CellValue == null) return cell.InnerText;
                            if (cell?.DataType != null && cell.DataType == CellValues.SharedString)
                            {
                                if (!int.TryParse(cell.InnerText, out var id)) return null;
                                var item = excelDocument?.WorkbookPart?.SharedStringTablePart?.SharedStringTable
                                    .Elements<SharedStringItem>().ElementAt(id);
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
                        }).ToArray());
                    }

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
    }
}

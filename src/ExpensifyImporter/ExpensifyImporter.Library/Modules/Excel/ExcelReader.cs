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


        public async Task<string> ReadAsJsonAsync(string path)
        {
            var workSheetList = new List<List<string[]>>();
            try
            {
                //Lets open the existing excel file and read through its content . Open the excel using openxml sdk
                using var excelDocument = SpreadsheetDocument.Open(path, false);

                var documentBody = excelDocument?.WorkbookPart?.Workbook;

                var worksheetParts = documentBody?.WorkbookPart?.WorksheetParts.ToList();

                if (worksheetParts == null) return await Task.FromResult(JsonSerializer.Serialize(workSheetList));

                //Needed for fetching the string values by id, usually stored in innertext property.
                var sharedStringTable = excelDocument?.WorkbookPart?.SharedStringTablePart?.SharedStringTable;

                workSheetList.AddRange(
                    await Task.WhenAll(
                        worksheetParts.Select(worksheetPart =>
                                GetPopulatedRowArrays(worksheetPart.Worksheet.Descendants<Row>().ToList(),
                                    sharedStringTable))
                            .ToArray()));

                return await Task.FromResult(JsonSerializer.Serialize(workSheetList));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        private Task<List<string?[]>> GetPopulatedRowArrays(List<Row> rows, SharedStringTable sharedStringTable)
        {
            return Task.FromResult(
                (from row in rows
                    select row.Elements<Cell>().Select(cell =>
                            sharedStringTable != null ? GetExcelCellValue(cell, sharedStringTable) : null)
                        .ToArray())
                .ToList()
            );
        }

        private string? GetExcelCellValue(Cell cell, SharedStringTable sharedStringTable)
        {
            if (cell.CellValue == null) return cell.InnerText;
            if (cell?.DataType != null && cell.DataType == CellValues.SharedString)
            {
                if (!int.TryParse(cell.InnerText, out var id)) return null;
                var item = sharedStringTable.Elements<SharedStringItem>().ElementAt(id);
                if (item.Text != null) return item.Text.Text;

                if (item.InnerText != null) return item.InnerText;

                if (item.InnerXml != null) return item.InnerXml;
            }
            else
            {
                return cell?.CellValue.Text;
            }

            return null;
        }
    }
}
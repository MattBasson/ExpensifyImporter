using System.Text.Json;
using ExpensifyImporter.Library.Modules.Excel.Domain;
using Microsoft.Extensions.Logging;

namespace ExpensifyImporter.Library.Modules.Excel
{
    public class ExcelDtoMapper
    {
        private readonly ILogger<ExcelDtoMapper> _logger;


        public ExcelDtoMapper(ILogger<ExcelDtoMapper> logger)
        {
            _logger = logger;
        }

        public async Task<List<ExcelSheet>> DeserializeAsync(string excelJson, bool firstRowHasHeaders = true)
        {
            var excelResponseDeserialized = JsonSerializer.Deserialize<IEnumerable<List<string[]>>>(excelJson);
            var book = new List<ExcelSheet>();
            if (excelResponseDeserialized == null) return book;
            foreach (var sheet in excelResponseDeserialized)
                book.Add(
                    new ExcelSheet(
                        await Task.WhenAll(sheet.Where((t, rowIndex) => rowIndex != 0 || !firstRowHasHeaders)
                            .Select(GetExcelRow).ToArray())));
            return book;
        }

        private Task<ExcelRow> GetExcelRow(string[] row)
        {
            return Task.FromResult(new ExcelRow(row.Select((cell, index) => new ExcelCell(index, cell))));
        }
    }
}
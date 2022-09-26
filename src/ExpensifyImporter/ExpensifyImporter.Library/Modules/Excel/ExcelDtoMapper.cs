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
            var excelResponseDeserialised = JsonSerializer.Deserialize<IEnumerable<List<string[]>>>(excelJson);
            var book = new List<ExcelSheet>();
            foreach (var sheet in excelResponseDeserialised)
            {
                var getExcelRowTasks = new List<Task<ExcelRow>>();
                
                for (var rowIndex = 0; rowIndex < sheet.Count; rowIndex++)
                {
                    if (rowIndex == 0 && firstRowHasHeaders) continue;
                    var row = sheet[rowIndex];
                    getExcelRowTasks.Add(GetExcelRow(rowIndex,row));
                }
                var excelSheet = new ExcelSheet( await Task.WhenAll(getExcelRowTasks.ToArray()) );
                book.Add(excelSheet);
            }
            return book;
        }

        private Task<ExcelRow> GetExcelRow(int index, string[] row)
        {
            return Task.FromResult(new ExcelRow(row.Select((cell, index) => new ExcelCell(index, cell))));

        }
    }
}
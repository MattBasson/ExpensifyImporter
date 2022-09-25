using ExpensifyImporter.Library.Modules.Excel.Domain;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ExpensifyImporter.Library.Modules.Excel
{
    public class ExcelDtoMapper
    {
        private readonly ILogger<ExcelDtoMapper> _logger;
    
        

        public ExcelDtoMapper(ILogger<ExcelDtoMapper> logger)
        {
            _logger = logger;            
        }

        public List<ExcelSheet> Deserialize(string excelJson,bool firstRowHasHeaders = true)
        {
            var excelResponseDeserialised = JsonSerializer.Deserialize<List<List<string[]>>>(excelJson);
            var book = new List<ExcelSheet>();
            foreach (var sheet in excelResponseDeserialised)
            {
                var excelSheet = new ExcelSheet();
                int rowCounter = 0;
               
                foreach (var row in sheet)
                {
                    if (rowCounter == 0 && firstRowHasHeaders)
                    {
                        rowCounter++;
                        continue;
                    }
                    var excelRow = new ExcelRow(); 
                    for (int cellIndex = 0; cellIndex < row.Length; cellIndex++)
                    {
                         excelRow.Add(new ExcelCell(cellIndex, row[cellIndex]));
                                                
                    }
                    excelSheet.Add(excelRow);
                    rowCounter++;
                }
                book.Add(excelSheet);
            }
            return book;
        }

    }
}

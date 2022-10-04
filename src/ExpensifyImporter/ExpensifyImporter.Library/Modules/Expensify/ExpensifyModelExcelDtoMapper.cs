using System.Collections.Concurrent;
using System.Globalization;
using DocumentFormat.OpenXml.Spreadsheet;
using ExpensifyImporter.Database.Domain;
using ExpensifyImporter.Library.Modules.Excel.Domain;
using Microsoft.Extensions.Logging;

namespace ExpensifyImporter.Library.Modules.Expensify
{
    public class ExpensifyModelExcelDtoMapper
    {
        private readonly ILogger<ExpensifyModelExcelDtoMapper> _logger;

        public ExpensifyModelExcelDtoMapper(ILogger<ExpensifyModelExcelDtoMapper> logger)
        {
            _logger = logger;
        }


        public async Task<List<Expense>> MapExpensesAsync(List<ExcelSheet> excelBook)
        {
            var expenses = new List<Expense>();

            var result = await Task.WhenAll(excelBook
                .Select(excelSheet => Task.Run(() => excelSheet.Select(selector: GetExpense).ToList())).ToArray());            
            foreach (var expenseSheet in result)
            {
                expenses.AddRange(expenseSheet);
            }
            return expenses;
        }

        private Expense GetExpense(ExcelRow excelRow)
        {
            return new Expense()
            {
                ReceiptId = int.Parse(excelRow["A"]?.CellValue ?? "0"),
                TransactionDateTime = DateTime.ParseExact(excelRow["B"]?.CellValue ?? string.Empty,
                    "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
                Merchant = excelRow["C"]?.CellValue,
                Amount = decimal.Parse(excelRow["D"]?.CellValue ?? "0",NumberStyles.Currency),
                Category = excelRow["E"]?.CellValue,                
                ReceiptUrl = excelRow["F"]?.CellValue
            };
        }
    }
}
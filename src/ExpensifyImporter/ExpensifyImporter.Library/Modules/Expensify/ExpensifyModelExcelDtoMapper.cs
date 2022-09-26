using System.Collections.Concurrent;
using System.Globalization;
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

        public List<Expense> MapExpenses(List<ExcelSheet> excelBook, bool firstRowHasHeaders = true)
        {
            var expenses = new ConcurrentBag<Expense>();

            foreach (var excelSheet in excelBook)
                Parallel.For(0, excelSheet.Count,
                    excelRowIndex =>
                    {
                        if (excelRowIndex == 0 && firstRowHasHeaders) return;

                        var excelRow = excelSheet[excelRowIndex];
                        expenses.Add(new Expense
                        {
                            ExpenseId = int.Parse(excelRow[0]?.CellValue ?? "0"),
                            TransactionDateTime = DateTime.ParseExact(excelRow[1]?.CellValue ?? string.Empty,
                                "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
                            Merchant = excelRow[2]?.CellValue,
                            Amount = decimal.Parse(excelRow[3]?.CellValue ?? "0"),
                            Category = excelRow[4]?.CellValue,
                            Description = excelRow[5]?.CellValue,
                            ReceiptUrl = excelRow[6]?.CellValue
                        });
                    });
            return expenses.ToList();
        }
    }
}
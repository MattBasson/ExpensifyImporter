﻿using System.Collections.Concurrent;
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


        public async Task<List<Expense>> MapExpensesAsync(List<ExcelSheet> excelBook)
        {
            var expenses = new List<Expense>();

            foreach (var excelSheet in excelBook)
            {
                expenses.AddRange(
                    await Task.WhenAll(
                        excelSheet.Select(GetExpense).ToArray()));
            }

            return expenses;
        }

        private Task<Expense> GetExpense(ExcelRow excelRow)
        {
            return Task.FromResult(new Expense()
            {
                ReceiptId = int.Parse(excelRow[0]?.CellValue ?? "0"),
                TransactionDateTime = DateTime.ParseExact(excelRow[1]?.CellValue ?? string.Empty,
                    "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
                Merchant = excelRow[2]?.CellValue,
                Amount = decimal.Parse(excelRow[3]?.CellValue ?? "0",NumberStyles.Currency),
                Category = excelRow[4]?.CellValue,                
                ReceiptUrl = excelRow[5]?.CellValue
            });
        }
    }
}
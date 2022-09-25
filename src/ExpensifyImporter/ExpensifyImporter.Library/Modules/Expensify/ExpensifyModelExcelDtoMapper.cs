using DocumentFormat.OpenXml.Office2013.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using ExpensifyImporter.Database.Domain;
using ExpensifyImporter.Library.Modules.Excel;
using ExpensifyImporter.Library.Modules.Excel.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var expenses = new List<Expense>();

            foreach (var excelSheet in excelBook)
            {
                int rowCounter = 0;
                foreach (var excelRow in excelSheet)
                {
                    if (rowCounter == 0 && firstRowHasHeaders)
                    {
                        rowCounter++;
                        continue;
                    }
                    var expense = new Expense();
                    expense.ExpenseId = int.Parse(excelRow[0]?.CellValue);
                    expense.ReceiptId = int.Parse(excelRow[1]?.CellValue);
                    expense.TransactionDateTime = DateTime.ParseExact(excelRow[2]?.CellValue, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                    expense.Merchant = excelRow[3]?.CellValue;
                    expense.Amount = decimal.Parse(excelRow[4]?.CellValue);
                    expense.Category = excelRow[5]?.CellValue;
                    expense.Description = excelRow[6]?.CellValue;
                    expense.ReceiptUrl = excelRow[7]?.CellValue;
                    expenses.Add(expense);
                    rowCounter++;
                }

            }
            return expenses;
        }

        

    }
}

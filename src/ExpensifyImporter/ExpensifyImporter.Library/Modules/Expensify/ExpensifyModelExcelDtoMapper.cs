using DocumentFormat.OpenXml.Office2013.Excel;
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
                    if(rowCounter == 0 && firstRowHasHeaders) {
                        rowCounter++;
                        continue;
                    }
                    var expense = new Expense();                    
                    int cellCounter = 0;
                    foreach(var excelCell in excelRow)
                    {
                        if (excelCell == null)
                        {
                            cellCounter++;
                            continue;
                        }                       

                        switch (cellCounter)
                        {
                            case 0:
                                expense.ExpenseId = int.Parse(excelCell.CellValue);
                                break;
                            case 1:
                                expense.ReceiptId = int.Parse(excelCell.CellValue);
                                break;
                            case 2:
                                expense.TransactionDateTime = DateTime.ParseExact(excelCell.CellValue, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                                break;
                            case 3:
                                expense.Merchant = excelCell.CellValue;
                                break;
                            case 4:
                                expense.Amount = decimal.Parse(excelCell.CellValue);
                                break;
                            case 5:
                                expense.Category = excelCell.CellValue;
                                break;
                            case 6:
                                expense.Description = excelCell.CellValue;
                                break;
                            case 7:
                                expense.ReceiptUrl = excelCell.CellValue;
                                break;
                        }

                        cellCounter++;
                    }
                    expenses.Add(expense);
                    rowCounter++;
                }
            }
            return expenses;
        }

        

    }
}

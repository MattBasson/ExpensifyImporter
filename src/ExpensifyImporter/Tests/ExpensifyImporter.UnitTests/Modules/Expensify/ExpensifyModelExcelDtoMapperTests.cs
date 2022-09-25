﻿using ExpensifyImporter.Library.Modules.Excel.Domain;
using Microsoft.Extensions.Logging;
using NSubstitute;
using ExpensifyImporter.Library.Modules.Expensify;
using ExpensifyImporter.Database.Domain;
using System.Globalization;
using FluentAssertions;

namespace ExpensifyImporter.UnitTests.Modules.Expensify
{
    public class ExpensifyModelExcelDtoMapperTests
    {
        [Fact]
        public void Mapping_ExcelBook_Collection_Yields_Correct_Expense_List()
        {
            //Arrange
            var exccelPocoList = new List<ExcelSheet>()
            {
                new ExcelSheet()
                {
                    new ExcelRow()
                    {
                        new ExcelCell(0,"ExpenseId"),
                        new ExcelCell(1,"ReceiptId"),
                        new ExcelCell(2,"TimeStamp"),
                        new ExcelCell(3,"Merchant"),
                        new ExcelCell(4,"Amount"),
                        new ExcelCell(5,"Category"),
                        new ExcelCell(6,"Description"),
                        new ExcelCell(7,"ReceiptUrl"),
                    },
                    new ExcelRow()
                    {
                        new ExcelCell(0,"1"),
                        new ExcelCell(1,"1"),
                        new ExcelCell(2,"2017-04-01 12:00:00"),
                        new ExcelCell(3,"Costa"),
                        new ExcelCell(4,"11.1"),
                        new ExcelCell(5,"Meals"),
                        new ExcelCell(6,""),
                        new ExcelCell(7,"https://www.expensify.com/receipts/w_8c12334126141c5a124ef131.jpg"),
                    },
                    new ExcelRow()
                    {
                        new ExcelCell(0,"2"),
                        new ExcelCell(1,"2"),
                        new ExcelCell(2,"2017-04-02 12:00:00"),
                        new ExcelCell(3,"Mcdonalds"),
                        new ExcelCell(4,"12.2"),
                        new ExcelCell(5,"Meals"),
                        new ExcelCell(6,""),
                        new ExcelCell(7,"https://www.expensify.com/receipts/w_8c12334126141c5a124ef132.jpg"),
                    },
                    new ExcelRow()
                    {
                        new ExcelCell(0,"3"),
                        new ExcelCell(1,"3"),
                        new ExcelCell(2,"2017-04-03 12:00:00"),
                        new ExcelCell(3,"Eat"),
                        new ExcelCell(4,"13.3"),
                        new ExcelCell(5,"Meals"),
                        new ExcelCell(6,""),
                        new ExcelCell(7,"https://www.expensify.com/receipts/w_8c12334126141c5a124ef133.jpg"),
                    },
                    new ExcelRow()
                    {
                        new ExcelCell(0,"4"),
                        new ExcelCell(1,"4"),
                        new ExcelCell(2,"2017-04-04 12:00:00"),
                        new ExcelCell(3,"Wasabi"),
                        new ExcelCell(4,"14.4"),
                        new ExcelCell(5,"Meals"),
                        new ExcelCell(6,""),
                        new ExcelCell(7,"https://www.expensify.com/receipts/w_8c12334126141c5a124ef134.jpg"),
                    }

                }
            };

            var mapper = new ExpensifyModelExcelDtoMapper(Substitute.For<ILogger<ExpensifyModelExcelDtoMapper>>());

            var expectedExpenseList = new List<Expense>()
            {
                new Expense()
                {                    
                    ExpenseId=1,
                    ReceiptId=1,
                    TransactionDateTime = DateTime.ParseExact("2017-04-01 12:00:00","yyyy-MM-dd HH:mm:ss",CultureInfo.InvariantCulture),
                    Merchant="Costa",
                    Amount = 11.1M,
                    Category ="Meals",
                    Description="",
                    ReceiptUrl = "https://www.expensify.com/receipts/w_8c12334126141c5a124ef131.jpg"
                },
                new Expense()
                {                    
                    ExpenseId=2,
                    ReceiptId=2,
                    TransactionDateTime = DateTime.ParseExact("2017-04-02 12:00:00","yyyy-MM-dd HH:mm:ss",CultureInfo.InvariantCulture),
                    Merchant="Mcdonalds",
                    Amount = 12.2M,
                    Category ="Meals",
                    Description="",
                    ReceiptUrl = "https://www.expensify.com/receipts/w_8c12334126141c5a124ef132.jpg"
                },
                new Expense()
                {                    
                    ExpenseId=3,
                    ReceiptId=3,
                    TransactionDateTime = DateTime.ParseExact("2017-04-03 12:00:00","yyyy-MM-dd HH:mm:ss",CultureInfo.InvariantCulture),
                    Merchant="Eat",
                    Amount = 13.3M,
                    Category ="Meals",
                    Description="",
                    ReceiptUrl = "https://www.expensify.com/receipts/w_8c12334126141c5a124ef133.jpg"
                },
                new Expense()
                {                    
                    ExpenseId=4,
                    ReceiptId=4,
                    TransactionDateTime = DateTime.ParseExact("2017-04-04 12:00:00","yyyy-MM-dd HH:mm:ss",CultureInfo.InvariantCulture),
                    Merchant="Wasabi",
                    Amount = 14.4M,
                    Category ="Meals",
                    Description="",
                    ReceiptUrl = "https://www.expensify.com/receipts/w_8c12334126141c5a124ef134.jpg"
                }
            };

            //Act 
            var expenses = mapper.MapExpenses(exccelPocoList);

            //Assert
            expenses.Should().BeEquivalentTo(expectedExpenseList, opt => opt.Excluding(e => e.Id));
        }
    }
}

using ExpensifyImporter.Library.Modules.Excel.Domain;
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
                        new ExcelCell(1,"TimeStamp"),
                        new ExcelCell(2,"Merchant"),
                        new ExcelCell(3,"Amount"),
                        new ExcelCell(4,"Category"),
                        new ExcelCell(5,"Description"),
                        new ExcelCell(6,"ReceiptUrl"),
                    },
                    new ExcelRow()
                    {
                        new ExcelCell(0,"1"),                        
                        new ExcelCell(1,"2017-04-01 12:00:00"),
                        new ExcelCell(2,"Costa"),
                        new ExcelCell(3,"11.1"),
                        new ExcelCell(4,"Meals"),
                        new ExcelCell(5,""),
                        new ExcelCell(6,"https://www.expensify.com/receipts/w_8c12334126141c5a124ef131.jpg"),
                    },
                    new ExcelRow()
                    {
                        new ExcelCell(0,"2"),                        
                        new ExcelCell(1,"2017-04-02 12:00:00"),
                        new ExcelCell(2,"Mcdonalds"),
                        new ExcelCell(3,"12.2"),
                        new ExcelCell(4,"Meals"),
                        new ExcelCell(5,""),
                        new ExcelCell(6,"https://www.expensify.com/receipts/w_8c12334126141c5a124ef132.jpg"),
                    },
                    new ExcelRow()
                    {
                        new ExcelCell(0,"3"),                        
                        new ExcelCell(1,"2017-04-03 12:00:00"),
                        new ExcelCell(2,"Eat"),
                        new ExcelCell(3,"13.3"),
                        new ExcelCell(4,"Meals"),
                        new ExcelCell(5,""),
                        new ExcelCell(6,"https://www.expensify.com/receipts/w_8c12334126141c5a124ef133.jpg"),
                    },
                    new ExcelRow()
                    {
                        new ExcelCell(0,"4"),                        
                        new ExcelCell(1,"2017-04-04 12:00:00"),
                        new ExcelCell(2,"Wasabi"),
                        new ExcelCell(3,"14.4"),
                        new ExcelCell(4,"Meals"),
                        new ExcelCell(5,""),
                        new ExcelCell(6,"https://www.expensify.com/receipts/w_8c12334126141c5a124ef134.jpg"),
                    }

                }
            };

            var mapper = new ExpensifyModelExcelDtoMapper(Substitute.For<ILogger<ExpensifyModelExcelDtoMapper>>());

            var expectedExpenseList = new List<Expense>()
            {
                new Expense()
                {                    
                    ExpenseId=1,                    
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

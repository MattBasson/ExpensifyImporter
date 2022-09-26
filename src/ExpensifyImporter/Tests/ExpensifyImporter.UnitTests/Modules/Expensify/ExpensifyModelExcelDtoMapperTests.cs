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
        public async Task Mapping_ExcelBook_Collection_Yields_Correct_Expense_List()
        {
            //Arrange
            var exccelPocoList = new List<ExcelSheet>()
            {
                new()
                {
                    new ExcelRow()
                    {
                        new(0,"ExpenseId"),                        
                        new(1,"TimeStamp"),
                        new(2,"Merchant"),
                        new(3,"Amount"),
                        new(4,"Category"),
                        new(5,"Description"),
                        new(6,"ReceiptUrl"),
                    },
                    new ExcelRow()
                    {
                        new(0,"1"),                        
                        new(1,"2017-04-01 12:00:00"),
                        new(2,"Costa"),
                        new(3,"11.1"),
                        new(4,"Meals"),
                        new(5,""),
                        new(6,"https://www.expensify.com/receipts/w_8c12334126141c5a124ef131.jpg"),
                    },
                    new ExcelRow()
                    {
                        new(0,"2"),                        
                        new(1,"2017-04-02 12:00:00"),
                        new(2,"Mcdonalds"),
                        new(3,"12.2"),
                        new(4,"Meals"),
                        new(5,""),
                        new(6,"https://www.expensify.com/receipts/w_8c12334126141c5a124ef132.jpg"),
                    },
                    new ExcelRow()
                    {
                        new(0,"3"),                        
                        new(1,"2017-04-03 12:00:00"),
                        new(2,"Eat"),
                        new(3,"13.3"),
                        new(4,"Meals"),
                        new(5,""),
                        new(6,"https://www.expensify.com/receipts/w_8c12334126141c5a124ef133.jpg"),
                    },
                    new ExcelRow()
                    {
                        new(0,"4"),                        
                        new(1,"2017-04-04 12:00:00"),
                        new(2,"Wasabi"),
                        new(3,"14.4"),
                        new(4,"Meals"),
                        new(5,""),
                        new(6,"https://www.expensify.com/receipts/w_8c12334126141c5a124ef134.jpg"),
                    }

                }
            };

            var mapper = new ExpensifyModelExcelDtoMapper(Substitute.For<ILogger<ExpensifyModelExcelDtoMapper>>());

            var expectedExpenseList = new List<Expense>()
            {
                new()
                {                    
                    ExpenseId=1,                    
                    TransactionDateTime = DateTime.ParseExact("2017-04-01 12:00:00","yyyy-MM-dd HH:mm:ss",CultureInfo.InvariantCulture),
                    Merchant="Costa",
                    Amount = 11.1M,
                    Category ="Meals",
                    Description="",
                    ReceiptUrl = "https://www.expensify.com/receipts/w_8c12334126141c5a124ef131.jpg"
                },
                new()
                {                    
                    ExpenseId=2,                    
                    TransactionDateTime = DateTime.ParseExact("2017-04-02 12:00:00","yyyy-MM-dd HH:mm:ss",CultureInfo.InvariantCulture),
                    Merchant="Mcdonalds",
                    Amount = 12.2M,
                    Category ="Meals",
                    Description="",
                    ReceiptUrl = "https://www.expensify.com/receipts/w_8c12334126141c5a124ef132.jpg"
                },
                new()
                {                    
                    ExpenseId=3,                    
                    TransactionDateTime = DateTime.ParseExact("2017-04-03 12:00:00","yyyy-MM-dd HH:mm:ss",CultureInfo.InvariantCulture),
                    Merchant="Eat",
                    Amount = 13.3M,
                    Category ="Meals",
                    Description="",
                    ReceiptUrl = "https://www.expensify.com/receipts/w_8c12334126141c5a124ef133.jpg"
                },
                new()
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
            var expenses = await mapper.MapExpensesAsync(exccelPocoList);

            //Assert
            expenses.Should().BeEquivalentTo(expectedExpenseList, opt => opt.Excluding(e => e.Id));
        }
    }
}

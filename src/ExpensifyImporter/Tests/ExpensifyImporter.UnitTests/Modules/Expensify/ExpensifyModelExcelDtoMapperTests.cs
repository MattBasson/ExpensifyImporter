using System.Globalization;
using ExpensifyImporter.Database.Domain;
using ExpensifyImporter.Library.Modules.Excel.Domain;
using ExpensifyImporter.Library.Modules.Expensify;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace ExpensifyImporter.UnitTests.Modules.Expensify;

public class ExpensifyModelExcelDtoMapperTests
{
    [Fact]
    public async Task Mapping_ExcelBook_Collection_Yields_Correct_Expense_List()
    {
        //Arrange
        var exccelPocoList = new List<ExcelSheet>
        {
            new()
            {
                new ExcelRow
                {
                    new(0, "1"),
                    new(1, "2017-04-01 12:00:00"),
                    new(2, "Costa"),
                    new(3, "11.1"),
                    new(4, "Meals"),                    
                    new(5, "https://www.expensify.com/receipts/w_8c12334126141c5a124ef131.jpg")
                },
                new ExcelRow
                {
                    new(0, "2"),
                    new(1, "2017-04-02 12:00:00"),
                    new(2, "Mcdonalds"),
                    new(3, "12.2"),
                    new(4, "Meals"),
                    new(5, "https://www.expensify.com/receipts/w_8c12334126141c5a124ef132.jpg")
                },
                new ExcelRow
                {
                    new(0, "3"),
                    new(1, "2017-04-03 12:00:00"),
                    new(2, "Eat"),
                    new(3, "13.3"),
                    new(4, "Meals"),                    
                    new(5, "https://www.expensify.com/receipts/w_8c12334126141c5a124ef133.jpg")
                },
                new ExcelRow
                {
                    new(0, "4"),
                    new(1, "2017-04-04 12:00:00"),
                    new(2, "Wasabi"),
                    new(3, "14.4"),
                    new(4, "Meals"),                    
                    new(5, "https://www.expensify.com/receipts/w_8c12334126141c5a124ef134.jpg")
                }
            }
        };

        var mapper = new ExpensifyModelExcelDtoMapper(Substitute.For<ILogger<ExpensifyModelExcelDtoMapper>>());

        var expectedExpenseList = new List<Expense>
        {
            new()
            {
                ReceiptId = 1,
                TransactionDateTime = DateTime.ParseExact("2017-04-01 12:00:00", "yyyy-MM-dd HH:mm:ss",
                    CultureInfo.InvariantCulture),
                Merchant = "Costa",
                Amount = 11.1M,
                Category = "Meals",                
                ReceiptUrl = "https://www.expensify.com/receipts/w_8c12334126141c5a124ef131.jpg"
            },
            new()
            {
                ReceiptId = 2,
                TransactionDateTime = DateTime.ParseExact("2017-04-02 12:00:00", "yyyy-MM-dd HH:mm:ss",
                    CultureInfo.InvariantCulture),
                Merchant = "Mcdonalds",
                Amount = 12.2M,
                Category = "Meals",
                ReceiptUrl = "https://www.expensify.com/receipts/w_8c12334126141c5a124ef132.jpg"
            },
            new()
            {
                ReceiptId = 3,
                TransactionDateTime = DateTime.ParseExact("2017-04-03 12:00:00", "yyyy-MM-dd HH:mm:ss",
                    CultureInfo.InvariantCulture),
                Merchant = "Eat",
                Amount = 13.3M,
                Category = "Meals",
                ReceiptUrl = "https://www.expensify.com/receipts/w_8c12334126141c5a124ef133.jpg"
            },
            new()
            {
                ReceiptId = 4,
                TransactionDateTime = DateTime.ParseExact("2017-04-04 12:00:00", "yyyy-MM-dd HH:mm:ss",
                    CultureInfo.InvariantCulture),
                Merchant = "Wasabi",
                Amount = 14.4M,
                Category = "Meals",
                ReceiptUrl = "https://www.expensify.com/receipts/w_8c12334126141c5a124ef134.jpg"
            }
        };

        //Act 
        var expenses = await mapper.MapExpensesAsync(exccelPocoList);

        //Assert
        expenses.Should().BeEquivalentTo(expectedExpenseList, opt => opt.Excluding(e => e.Id));
    }
}
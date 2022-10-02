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
                    new( "1","A1","A"),
                    new( "2017-04-01 12:00:00","B1","B"),
                    new( "Costa","C1","C"),
                    new( "11.1","D1","D"),
                    new( "Meals","E1","E"),                    
                    new( "https://www.expensify.com/receipts/w_8c12334126141c5a124ef131.jpg", "F1", "F")
                },
                new ExcelRow
                {
                    new( "2", "A2", "A"),
                    new( "2017-04-02 12:00:00","B2","B"),
                    new( "Mcdonalds", "C2","C"),
                    new( "12.2", "D2","D"),
                    new( "Meals", "E2", "E"),
                    new( "https://www.expensify.com/receipts/w_8c12334126141c5a124ef132.jpg", "F2", "F")
                },
                new ExcelRow
                {
                    new( "3", "A3", "A"),
                    new( "2017-04-03 12:00:00", "B3", "B"),
                    new( "Eat", "C3", "C"),
                    new( "13.3", "D3", "D"),
                    new( "Meals", "E3", "E"),                    
                    new( "https://www.expensify.com/receipts/w_8c12334126141c5a124ef133.jpg", "F3", "F")
                },
                new ExcelRow
                {
                    new( "4", "A4", "A"),
                    new( "2017-04-04 12:00:00", "B4", "B"),
                    new( "Wasabi","C4","C"),
                    new( "14.4","D4", "D"),
                    new( "Meals", "E4", "E"),                    
                    new( "https://www.expensify.com/receipts/w_8c12334126141c5a124ef134.jpg", "F4", "F")
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
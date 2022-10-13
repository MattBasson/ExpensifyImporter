using System.Globalization;
using System.Reflection;
using ExpensifyImporter.Database.Domain;
using ExpensifyImporter.Library.Modules.Database;
using ExpensifyImporter.Library.Modules.Expensify;
using ExpensifyImporter.Library.Modules.IO;
using ExpensifyImporter.UnitTests.Modules.IO;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace ExpensifyImporter.UnitTests.Modules.Database
{
    public class ExpenseImageBatchCommandTests
    {
        private readonly List<Expense> _expenseList;

        public ExpenseImageBatchCommandTests()
        {
             _expenseList = new List<Expense>
            {
                new()
                {
                    TransactionDateTime = DateTime.ParseExact("2017-03-17 12:00:00", "yyyy-MM-dd HH:mm:ss",
                        CultureInfo.InvariantCulture),
                    Merchant = "Mcdonald's",
                    Amount = 6.67M,
                    Category = "Meals",
                    ReceiptUrl = "https://www.expensify.com/receipts/w_cb2240f3c271914fea730ad968a7b4eca17344exxxx1.jpg",
                    ReceiptId = 1
                },
                new()
                {
                    TransactionDateTime = DateTime.ParseExact("2017-03-17 12:00:00", "yyyy-MM-dd HH:mm:ss",
                        CultureInfo.InvariantCulture),
                    Merchant = "Flat Iron Steak",
                    Amount = 28.25M,
                    Category = "Meals",
                    ReceiptUrl = "https://www.expensify.com/receipts/w_a9de11dea8a62a8b2976d5ab626b252b1c23c8exxxx2.jpg",
                    ReceiptId = 2
                },
                new()
                {
                    TransactionDateTime = DateTime.ParseExact("2017-03-17 12:00:00", "yyyy-MM-dd HH:mm:ss",
                        CultureInfo.InvariantCulture),
                    Merchant = "Flat Iron Steak",
                    Amount = 28.25M,
                    Category = "Meals",
                    ReceiptUrl = "https://www.expensify.com/receipts/w_eb4140ba23c9088d021ce70a54e8490618e7afexxxx3.jpg",
                    ReceiptId = 3
                },
                new()
                {
                    TransactionDateTime = DateTime.ParseExact("2017-03-22 12:00:00", "yyyy-MM-dd HH:mm:ss",
                        CultureInfo.InvariantCulture),
                    Merchant = "Pret A Manger",
                    Amount = 6.43M,
                    Category = "Meals",
                    ReceiptUrl = "https://www.expensify.com/receipts/w_ed3cedc0fc51d18a440b3500e9e750e05eebc8axxxx4.jpg",
                    ReceiptId = 4
                },
                new()
                {
                    TransactionDateTime = DateTime.ParseExact("2017-03-21 12:00:00", "yyyy-MM-dd HH:mm:ss",
                        CultureInfo.InvariantCulture),
                    Merchant = "Pret A Manger",
                    Amount = 6.43M,
                    Category = "Meals",
                    ReceiptUrl = "https://www.expensify.com/receipts/w_ebe8a056fb696b2b2c471a6bf84dcd624a2e760xxxx5.jpg",
                    ReceiptId = 5
                },
                new()
                {
                    TransactionDateTime = DateTime.ParseExact("2017-03-21 12:00:00", "yyyy-MM-dd HH:mm:ss",
                        CultureInfo.InvariantCulture),
                    Merchant = "Eat.",
                    Amount = 4.99M,
                    Category = "Meals",
                    ReceiptUrl = "https://www.expensify.com/receipts/w_8b2338ffc1ff63e83e55d9d4126141c5a124ef1xxxx6.jpg",
                    ReceiptId = 6
                },
                new()
                {
                    TransactionDateTime = DateTime.ParseExact("2017-03-20 12:00:00", "yyyy-MM-dd HH:mm:ss",
                        CultureInfo.InvariantCulture),
                    Merchant = "Pret A Manger",
                    Amount = 6.44M,
                    Category = "Other",
                    ReceiptUrl = "https://www.expensify.com/receipts/w_1937eb19d0c3a6aad1526cc0b00d2ce799ee360xxxx7.jpg",
                    ReceiptId = 7
                },
                new()
                {
                    TransactionDateTime = DateTime.ParseExact("2017-03-20 12:00:00", "yyyy-MM-dd HH:mm:ss",
                        CultureInfo.InvariantCulture),
                    Merchant = "Five Guys Burgers And Fries",
                    Amount = 16.36M,
                    Category = "Meals",
                    ReceiptUrl = "https://www.expensify.com/receipts/w_f2e2fb0228c36de6bf8225ff3746f5f01acf4a1xxxx8.jpg",
                    ReceiptId = 8
                },
                new()
                {
                    TransactionDateTime = DateTime.ParseExact("2017-03-23 12:00:00", "yyyy-MM-dd HH:mm:ss",
                        CultureInfo.InvariantCulture),
                    Merchant = "Pret A Manger",
                    Amount = 6.43M,
                    Category = "Meals",
                    ReceiptUrl = "https://www.expensify.com/receipts/w_a445fd27951f1d516f0b67daa93885b95e755e7xxxx9.jpg",
                    ReceiptId = 9
                }
            };
        }

        [Fact]
        public async Task Given_Batch_Of_Download_Results_Is_Saved_To_Database_Successfully()
        {
            //Arrange
            var dbContext = SQLiteHelper.CreateSqliteContext();
            await dbContext.Expense.AddRangeAsync(_expenseList);
            await dbContext.SaveChangesAsync();

            var image1 = await EmbeddedData.GetByteArrayAsync(
                $"ExpensifyImporter.UnitTests.Modules.IO.Data.{Constants.CatImageFile1}",
                Assembly.GetAssembly(typeof(ImageDownloaderTests)));
            
            var image2 = await EmbeddedData.GetByteArrayAsync(
                $"ExpensifyImporter.UnitTests.Modules.IO.Data.{Constants.CatImageFile2}",
                Assembly.GetAssembly(typeof(ImageDownloaderTests)));
            
            var image3 = await EmbeddedData.GetByteArrayAsync(
                $"ExpensifyImporter.UnitTests.Modules.IO.Data.{Constants.CatImageFile3}",
                Assembly.GetAssembly(typeof(ImageDownloaderTests)));

            var expectedDownloadResult = new List<ExpensifyImageDownloadResult>()
            {
                new(_expenseList[0].Id, image1),
                new(_expenseList[1].Id, image2),
                new(_expenseList[2].Id, image3),
            };
            
            var sut = new ExpenseImageBatchCommand(
                Substitute.For<ILogger<ExpenseImageBatchCommand>>(),
                dbContext);
            
            //Act
            var result = await sut.ExecuteAsync(expectedDownloadResult);


            //Assert
            result.Should().NotBe(0);
            result.Should().Be(3);

            var record1 = await dbContext.Expense.SingleAsync(s=>s.Id == _expenseList[0].Id);
            record1.ReceiptImage.Should().BeEquivalentTo(image1);
            var record2 = await dbContext.Expense.SingleAsync(s=>s.Id == _expenseList[1].Id);
            record2.ReceiptImage.Should().BeEquivalentTo(image2);
            var record3 = await dbContext.Expense.SingleAsync(s=>s.Id == _expenseList[2].Id);
            record3.ReceiptImage.Should().BeEquivalentTo(image3);
            
        }
    }
}
using ExpensifyImporter.Database.Domain;
using ExpensifyImporter.Library.Modules.Sequencing;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System.Globalization;
using ExpensifyImporter.Library.Modules.Database;
using ExpensifyImporter.Library.Modules.Expensify;
using ExpensifyImporter.Library.Modules.IO;
using ExpensifyImporter.UnitTests.Modules.IO.Fakes;
using Microsoft.EntityFrameworkCore;

namespace ExpensifyImporter.UnitTests.Modules.Sequencing
{
    public class ImageToDatabaseSequencerTests
    {
        private readonly List<Expense> _expenseList;

        public ImageToDatabaseSequencerTests()
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
                    ReceiptUrl = Constants.CatImageUrl1,
                    ReceiptId = 1

                },
                new()
                {
                    TransactionDateTime = DateTime.ParseExact("2017-03-17 12:00:00", "yyyy-MM-dd HH:mm:ss",
                        CultureInfo.InvariantCulture),
                    Merchant = "Flat Iron Steak",
                    Amount = 28.25M,
                    Category = "Meals",
                    ReceiptUrl = Constants.CatImageUrl2,
                    ReceiptId = 2
                },
                new()
                {
                    TransactionDateTime = DateTime.ParseExact("2017-03-17 12:00:00", "yyyy-MM-dd HH:mm:ss",
                        CultureInfo.InvariantCulture),
                    Merchant = "Flat Iron Steak",
                    Amount = 28.25M,
                    Category = "Meals",
                    ReceiptUrl = Constants.CatImageUrl3,
                    ReceiptId = 3
                },
                new()
                {
                    TransactionDateTime = DateTime.ParseExact("2017-03-22 12:00:00", "yyyy-MM-dd HH:mm:ss",
                        CultureInfo.InvariantCulture),
                    Merchant = "Pret A Manger",
                    Amount = 6.43M,
                    Category = "Meals",
                    ReceiptUrl = Constants.CatImageUrl1,
                    ReceiptId = 4
                },
                new()
                {
                    TransactionDateTime = DateTime.ParseExact("2017-03-21 12:00:00", "yyyy-MM-dd HH:mm:ss",
                        CultureInfo.InvariantCulture),
                    Merchant = "Pret A Manger",
                    Amount = 6.43M,
                    Category = "Meals",
                    ReceiptUrl = Constants.CatImageUrl2,
                    ReceiptId = 5

                },
                new()
                {
                    TransactionDateTime = DateTime.ParseExact("2017-03-21 12:00:00", "yyyy-MM-dd HH:mm:ss",
                        CultureInfo.InvariantCulture),
                    Merchant = "Eat.",
                    Amount = 4.99M,
                    Category = "Meals",
                    ReceiptUrl = Constants.CatImageUrl3,
                    ReceiptId = 6
                },
                new()
                {
                    TransactionDateTime = DateTime.ParseExact("2017-03-20 12:00:00", "yyyy-MM-dd HH:mm:ss",
                        CultureInfo.InvariantCulture),
                    Merchant = "Pret A Manger",
                    Amount = 6.44M,
                    Category = "Other",
                    ReceiptUrl = Constants.CatImageUrl1,
                    ReceiptId = 7
                },
                new()
                {
                    TransactionDateTime = DateTime.ParseExact("2017-03-20 12:00:00", "yyyy-MM-dd HH:mm:ss",
                        CultureInfo.InvariantCulture),
                    Merchant = "Five Guys Burgers And Fries",
                    Amount = 16.36M,
                    Category = "Meals",
                    ReceiptUrl = Constants.CatImageUrl2,
                    ReceiptId = 8
                },
                new()
                {
                    TransactionDateTime = DateTime.ParseExact("2017-03-23 12:00:00", "yyyy-MM-dd HH:mm:ss",
                        CultureInfo.InvariantCulture),
                    Merchant = "Pret A Manger",
                    Amount = 6.43M,
                    Category = "Meals",
                    ReceiptUrl = Constants.CatImageUrl3,
                    ReceiptId = 9
                }
            };
          
        }

        [Fact]
        public async Task When_Batch_Size_0_Ensure_All_Image_Paths_Set_in_Database()
        {
            //Arrange
            var dbContext = SQLiteHelper.CreateSqliteContext();
            await dbContext.Expense.AddRangeAsync(_expenseList);
            await dbContext.SaveChangesAsync();
            
            var sut = new ImageToDatabaseSequencer(
                Substitute.For<ILogger<ImageToDatabaseSequencer>>(),
                new ExpenseImageBatchQuery(Substitute.For<ILogger<ExpenseImageBatchQuery>>(),dbContext),
                new ExpensifyImageDownloader(Substitute.For<ILogger<ExpensifyImageDownloader>>(),
                    new ImageDownloader(Substitute.For<ILogger<ImageDownloader>>(),new HttpClient(new FakeImageDownloaderHttpMessageHandler()))),
                new ExpenseImageBatchCommand(Substitute.For<ILogger<ExpenseImageBatchCommand>>(),dbContext));

            //Act
            var response = await sut.ProcessAsync();


            //Assert
            var setImages = await dbContext.Expense.Where(w => w.ReceiptImage != null).ToListAsync();
            setImages.Count.Should().Be(9);
            response.Should().Be(9);


        }

        [Fact]
        public async Task When_Batch_Size_Set_Ensure_Only_Set_Number_Image_Paths_Set_in_Database()
        {
            //Arrange
            var dbContext = SQLiteHelper.CreateSqliteContext();
            await dbContext.Expense.AddRangeAsync(_expenseList);
            await dbContext.SaveChangesAsync();
            var imageToDatabaseSequencer = new ImageToDatabaseSequencer(
                Substitute.For<ILogger<ImageToDatabaseSequencer>>(),
                new ExpenseImageBatchQuery(Substitute.For<ILogger<ExpenseImageBatchQuery>>(),dbContext),
                new ExpensifyImageDownloader(Substitute.For<ILogger<ExpensifyImageDownloader>>(),
                    new ImageDownloader(Substitute.For<ILogger<ImageDownloader>>(),new HttpClient(new FakeImageDownloaderHttpMessageHandler()))),
                new ExpenseImageBatchCommand(Substitute.For<ILogger<ExpenseImageBatchCommand>>(),dbContext));

            //Act
            var response = await imageToDatabaseSequencer.ProcessAsync(4);


            //Assert
            response.Should().Be(4);
        }
    }
}

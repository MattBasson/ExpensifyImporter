using System.Reflection;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using ExpensifyImporter.Library.Modules.Database;
using ExpensifyImporter.Library.Modules.Expensify;
using ExpensifyImporter.Library.Modules.IO;
using ExpensifyImporter.UnitTests.Modules.IO;
using ExpensifyImporter.UnitTests.Modules.IO.Fakes;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace ExpensifyImporter.UnitTests.Modules.Expensify
{
    public class ExpensifyImageDownloaderTests
    {
        public ExpensifyImageDownloaderTests()
        {
        }

        [Fact]
        public async Task Given_A_Batch_It_Downloads_All_Items_In_Batch()
        {
            //Arrange
            var batch = new List<ExpenseImageBatchQueryResult>()
            {
                new(Guid.NewGuid(), "https://images.all-free-download.com/images/graphicwebp/cat_domestic_cat_sweet_269854.webp"),
                new(Guid.NewGuid(), "https://images.all-free-download.com/images/graphicwebp/cat_feline_cats_eye_220526.webp"),
                new(Guid.NewGuid(), "https://images.all-free-download.com/images/graphicwebp/cat_cats_eyes_cat_face_269574.webp")
            };
            const string EmbeddedDataPath = "ExpensifyImporter.UnitTests.Modules.IO.Data.";
            var expectedResult = new List<ExpensifyImageDownloadResult>()
            {
                new(batch[0].ExpenseId, await EmbeddedData.GetByteArrayAsync(
                    $"ExpensifyImporter.UnitTests.Modules.IO.Data.cat_domestic_cat_sweet_269854.webp",
                    Assembly.GetAssembly(typeof(ExpensifyImageDownloaderTests)))),
                new (batch[1].ExpenseId, await EmbeddedData.GetByteArrayAsync($"ExpensifyImporter.UnitTests.Modules.IO.Data.cat_feline_cats_eye_220526.webp",
                    Assembly.GetAssembly(typeof(ExpensifyImageDownloaderTests)))),
                new (batch[2].ExpenseId, await EmbeddedData.GetByteArrayAsync($"ExpensifyImporter.UnitTests.Modules.IO.Data.cat_cats_eyes_cat_face_269574.webp",
                    Assembly.GetAssembly(typeof(ExpensifyImageDownloaderTests)))),
            };
            
            var fakeImageDownloaderMessageHandler = new FakeImageDownloaderHttpMessageHandler();

            var sut = new ExpensifyImageDownloader(
                Substitute.For<ILogger<ExpensifyImageDownloader>>(),
                new ImageDownloader(Substitute.For<ILogger<ImageDownloader>>(),
                    new HttpClient(fakeImageDownloaderMessageHandler)));
            //Act
            var result = await sut.ExecuteAsync(batch);

            //Assert

            result.Any(a => a.FileContents == null).Should().BeFalse();
            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}
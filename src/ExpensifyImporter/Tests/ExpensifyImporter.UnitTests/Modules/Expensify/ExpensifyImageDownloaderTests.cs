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
                new(Guid.NewGuid(), Constants.CatImageUrl1,null),
                new(Guid.NewGuid(), Constants.CatImageUrl2,null),
                new(Guid.NewGuid(), Constants.CatImageUrl3, null)
            };
            const string EmbeddedDataPath = "ExpensifyImporter.UnitTests.Modules.IO.Data.";
            var expectedResult = new List<ExpensifyImageDownloadResult>()
            {
                new(batch[0].Id, await EmbeddedData.GetByteArrayAsync(
                    $"{EmbeddedDataPath}{Constants.CatImageFile1}",
                    Assembly.GetAssembly(typeof(ExpensifyImageDownloaderTests)))),
                new (batch[1].Id, await EmbeddedData.GetByteArrayAsync($"{EmbeddedDataPath}{Constants.CatImageFile2}",
                    Assembly.GetAssembly(typeof(ExpensifyImageDownloaderTests)))),
                new (batch[2].Id, await EmbeddedData.GetByteArrayAsync($"{EmbeddedDataPath}{Constants.CatImageFile3}",
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

        [Fact]
        public async Task Given_A_Batch_With_Duplicate_Urls_It_Downloads_All_Items_In_Batch()
        {
            //Arrange
            var batch = new List<ExpenseImageBatchQueryResult>()
            {
                new(Guid.NewGuid(), Constants.CatImageUrl1,null),
                new(Guid.NewGuid(), Constants.CatImageUrl1,null),
                new(Guid.NewGuid(), Constants.CatImageUrl1,null)
            };
            const string EmbeddedDataPath = "ExpensifyImporter.UnitTests.Modules.IO.Data.";
            var catImage1ByteArray = await EmbeddedData.GetByteArrayAsync(
                $"{EmbeddedDataPath}{Constants.CatImageFile1}",
                Assembly.GetAssembly(typeof(ExpensifyImageDownloaderTests)));
            var expectedResult = new List<ExpensifyImageDownloadResult>()
            {
                new(batch[0].Id, catImage1ByteArray),
                new(batch[1].Id, catImage1ByteArray),
                new(batch[2].Id, catImage1ByteArray),
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

        [Fact]
        public async Task Given_A_Batch_With_Null_Urls_It_Returns_All_Items_In_Batch()
        {
            //Arrange
            var batch = new List<ExpenseImageBatchQueryResult>()
            {
                new(Guid.NewGuid(), null,null),
                new(Guid.NewGuid(), null,null),
                new(Guid.NewGuid(), null,null)
            };
            
            var expectedResult = new List<ExpensifyImageDownloadResult>()
            {
                new(batch[0].Id, null),
                    
                new (batch[1].Id, null),
                    
                new (batch[2].Id,null),
            };

            var fakeImageDownloaderMessageHandler = new FakeImageDownloaderHttpMessageHandler();

            var sut = new ExpensifyImageDownloader(
                Substitute.For<ILogger<ExpensifyImageDownloader>>(),
                new ImageDownloader(Substitute.For<ILogger<ImageDownloader>>(),
                    new HttpClient(fakeImageDownloaderMessageHandler)));
            //Act
            var result = await sut.ExecuteAsync(batch);

            //Assert

            //result.Any(a => a.FileContents == null).Should().BeFalse();
            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}
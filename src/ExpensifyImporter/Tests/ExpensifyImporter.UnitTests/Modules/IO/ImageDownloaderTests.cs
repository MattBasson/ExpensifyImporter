using System.Reflection;
using ExpensifyImporter.Library.Modules.IO;
using ExpensifyImporter.UnitTests.Modules.IO.Fakes;
using FluentAssertions;
using Microsoft.Extensions.Logging;

using NSubstitute;

namespace ExpensifyImporter.UnitTests.Modules.IO
{
    public class ImageDownloaderTests
    {
        private readonly HttpClient _client;

        public ImageDownloaderTests()
        {
            _client = new HttpClient();
        }

        [Theory]
        [InlineData(Constants.CatImageUrl1, Constants.CatImageFile1)]
        [InlineData(Constants.CatImageUrl2, Constants.CatImageFile2)]
        [InlineData(Constants.CatImageUrl3, Constants.CatImageFile3)]
        public async Task When_Image_Download_It_Successfully_Downloads_Fake_Image(string url, string filename)
        {
            //Arrange 
            var expectedFileByteArray = await
                EmbeddedData.GetByteArrayAsync($"ExpensifyImporter.UnitTests.Modules.IO.Data.{filename}",
                    Assembly.GetAssembly(typeof(ImageDownloaderTests)));

        
            var httpClient = new HttpClient(new FakeImageDownloaderHttpMessageHandler());
            

            var sut = new ImageDownloader(Substitute.For<ILogger<ImageDownloader>>(), httpClient);

            //Act
            var result = await sut.ExecuteAsync(url);

            //Assert

            result.fileContents.Should().Equal(expectedFileByteArray);

            
        }
        
        //Todo: Move this to integration test.
        [Theory]
        [InlineData(Constants.CatImageUrl1, Constants.CatImageFile1)]
        [InlineData(Constants.CatImageUrl2, Constants.CatImageFile2)]
        [InlineData(Constants.CatImageUrl3, Constants.CatImageFile3)]
        public async Task When_Image_Download_It_Successfully_Downloads_Real_Image(string url, string filename)
        {
            //Arrange 
            var expectedFileByteArray = await
                EmbeddedData.GetByteArrayAsync($"ExpensifyImporter.UnitTests.Modules.IO.Data.{filename}",
                    Assembly.GetAssembly(typeof(ImageDownloaderTests)));
            
            
            var httpClient = new HttpClient();
            

            var sut = new ImageDownloader(Substitute.For<ILogger<ImageDownloader>>(), httpClient);

            //Act
            var result = await sut.ExecuteAsync(url);

            //Assert

            result.fileContents.Should().Equal(expectedFileByteArray);

            
        }
    }
}
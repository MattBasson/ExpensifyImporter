using System.Net;
using System.Reflection;
using ExpensifyImporter.Library.Modules.IO;
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
        [InlineData("https://images.all-free-download.com/images/graphicwebp/cat_domestic_cat_sweet_269854.webp",
            "cat_domestic_cat_sweet_269854.webp")]
        [InlineData("https://images.all-free-download.com/images/graphicwebp/cat_feline_cats_eye_220526.webp",
            "cat_feline_cats_eye_220526.webp")]
        [InlineData("https://images.all-free-download.com/images/graphicwebp/cat_cats_eyes_cat_face_269574.webp",
            "cat_cats_eyes_cat_face_269574.webp")]
 
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

            result.Should().Equal(expectedFileByteArray);

            
        }
    }

    public class FakeImageDownloaderHttpMessageHandler : HttpMessageHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            //get the filename from the request
            var fileName =
                request.RequestUri?.OriginalString[(request.RequestUri.OriginalString.LastIndexOf("/", StringComparison.Ordinal)+1)..];
            //Get the file byte array from embedded data.
            var fileByteArray = await EmbeddedData.GetByteArrayAsync($"ExpensifyImporter.UnitTests.Modules.IO.Data.{fileName}",
                Assembly.GetAssembly(typeof(ImageDownloaderTests)));
            if (fileByteArray != null)
            {
                //return the fileByteArray if it matches the request
                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ByteArrayContent(fileByteArray)
                };
            }
            //otherwise return no content
            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }
    }
}
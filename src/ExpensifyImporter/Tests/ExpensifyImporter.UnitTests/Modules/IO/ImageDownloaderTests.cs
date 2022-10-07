using System.Net;
using System.Net.Http.Headers;
using System.Reflection;
using ExpensifyImporter.Library.Modules.IO;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
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
            var expectedFileByteArray =
                EmbeddedData.GetByteArray($"ExpensifyImporter.UnitTests.Modules.IO.Data.{filename}",
                    Assembly.GetAssembly(typeof(ImageDownloaderTests)));
            
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                ).ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new ByteArrayContent(expectedFileByteArray)
                }).Verifiable();

            var httpClient = new HttpClient(handlerMock.Object);

            var sut = new ImageDownloader(Substitute.For<ILogger<ImageDownloader>>(), httpClient);

            //Act
            var result = await sut.ExecuteAsync(url);

            //Assert

            result.Should().Equal(expectedFileByteArray);
                            
            handlerMock.Protected().Verify(
                "SendAsync",
                Times.Exactly(1), // we expected a single external request
                ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Get  // we expected a GET request
                        && req.RequestUri == new Uri(url) // to this uri
                ),
                ItExpr.IsAny<CancellationToken>()
            );
        }
    }
}
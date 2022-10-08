using System.Net;
using System.Reflection;
using ExpensifyImporter.Library.Modules.IO;

namespace ExpensifyImporter.UnitTests.Modules.IO.Fakes
{
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
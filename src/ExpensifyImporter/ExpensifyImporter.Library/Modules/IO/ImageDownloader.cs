namespace ExpensifyImporter.Library.Modules.IO
{
    public record ImageDownloadResult(string url, byte[]? fileContents);
    public class ImageDownloader
    {
        private readonly ILogger<ImageDownloader> _logger;
        private readonly HttpClient _client;

        public ImageDownloader(ILogger<ImageDownloader> logger, HttpClient client)
        {
            _logger = logger;
            _client = client;
        }

        public async Task<ImageDownloadResult> ExecuteAsync(string url)
        {
            var fileContents = await DownloadImageAsync(url);

            return new ImageDownloadResult(url, fileContents);
        }

        private async Task<byte[]?> DownloadImageAsync(string url)
        {
            _logger.LogDebug("Downloading image {Url}",url);
            
            return await _client.GetByteArrayAsync(url);
        }
    }
}
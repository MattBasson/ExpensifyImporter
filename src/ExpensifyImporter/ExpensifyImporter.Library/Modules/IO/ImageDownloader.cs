namespace ExpensifyImporter.Library.Modules.IO
{
    public class ImageDownloader
    {
        private readonly ILogger<ImageDownloader> _logger;
        private readonly HttpClient _client;

        public ImageDownloader(ILogger<ImageDownloader> logger, HttpClient client)
        {
            _logger = logger;
            _client = client;
        }

        public async Task<byte[]?> ExecuteAsync(string url)
        {
            _logger.LogDebug("Downloading image {Url}",url);
            
            return await _client.GetByteArrayAsync(url);
        }
    }
}
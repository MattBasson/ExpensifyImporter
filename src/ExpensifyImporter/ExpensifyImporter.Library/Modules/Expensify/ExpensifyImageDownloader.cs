using ExpensifyImporter.Library.Modules.IO;

namespace ExpensifyImporter.Library.Modules.Expensify
{
    public record ExpensifyImageDownloadResult(Guid ExpenseId, byte[]? FileContents);
    public class ExpensifyImageDownloader
    {
        private readonly ILogger<ExpensifyImageDownloader> _logger;
        private readonly ImageDownloader _imageDownloader;

        public ExpensifyImageDownloader(ILogger<ExpensifyImageDownloader> logger, ImageDownloader imageDownloader)
        {
            _logger = logger;
            _imageDownloader = imageDownloader;
        }

        public async Task<IEnumerable<ExpensifyImageDownloadResult>> ExecuteAsync(IEnumerable<ExpenseImageBatchQueryResult> batch)
        {
            var tasks = batch.Select(expenseImageBatchQueryResult =>
                GetDownloadResult(expenseImageBatchQueryResult.ExpenseId, expenseImageBatchQueryResult.Url)).ToList();
           
            return await Task.WhenAll(tasks);
        }

        private async Task<ExpensifyImageDownloadResult> GetDownloadResult(Guid id, string? url)
        {
            if (url == null) return new ExpensifyImageDownloadResult(id, null);
            
            var downloadResult = await _imageDownloader.ExecuteAsync(url);

            return new ExpensifyImageDownloadResult(id, downloadResult.fileContents);

        }
        
    }
}
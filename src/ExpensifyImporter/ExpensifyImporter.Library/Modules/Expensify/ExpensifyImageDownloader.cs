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
            
            var tasks = batch.Select(expenseImageBatchQueryResult => _imageDownloader.ExecuteAsync(expenseImageBatchQueryResult.Url)).ToList();
            var result = await Task.WhenAll(tasks);

            return result.Select(downloadResult =>
                new ExpensifyImageDownloadResult(batch.First(f => f.Url == downloadResult.url).ExpenseId,
                    downloadResult.fileContents)).ToList();
        }
    }
}
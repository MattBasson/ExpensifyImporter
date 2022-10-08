
using ExpensifyImporter.Library.Modules.Expensify;

namespace ExpensifyImporter.Library.Modules.Sequencing
{
    public class ImageToDatabaseSequencer
    {
        private readonly ILogger<ImageToDatabaseSequencer> _logger;
        private readonly ExpenseImageBatchQuery _expenseImageBatchQuery;
        private readonly ExpensifyImageDownloader _expensifyImageDownloader;

        public ImageToDatabaseSequencer(
            ILogger<ImageToDatabaseSequencer> logger,
            ExpenseImageBatchQuery expenseImageBatchQuery,
            ExpensifyImageDownloader expensifyImageDownloader)
        {
            _logger = logger;
            _expenseImageBatchQuery = expenseImageBatchQuery;
            _expensifyImageDownloader = expensifyImageDownloader;
        }

        public async Task<int> ProcessAsync(int batchSize = 0)
        {
            // 1) Get dataset of items that have no image set (batch size sensitive)
            _logger.LogInformation("Get dataset of items that have no image set (batch size sensitive) : {BatchSize}", batchSize);
            //ExpenseImageBatchQuery returns ExpenseID array
            var query = await _expenseImageBatchQuery.ExecuteAsync(batchSize);


            // 2) Download images  return an array of ExpenseIds and byte arrays.
            _logger.LogInformation("Downloading image batch {BatchSize}", batchSize);
            var downloadResult = await _expensifyImageDownloader.ExecuteAsync(query);
            
            // 3) Asynchronous saving of array
            //return rows modifed
            //Expensse Batch Commaand
            

            return 0;
        }
    }
}

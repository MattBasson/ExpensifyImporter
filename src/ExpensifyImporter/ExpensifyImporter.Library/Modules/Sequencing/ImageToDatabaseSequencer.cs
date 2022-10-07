
namespace ExpensifyImporter.Library.Modules.Sequencing
{
    public class ImageToDatabaseSequencer
    {
        private readonly ILogger<ImageToDatabaseSequencer> _logger;
        private readonly ExpenseImageBatchQuery _expenseImageBatchQuery;

        public ImageToDatabaseSequencer(
            ILogger<ImageToDatabaseSequencer> logger,
            ExpenseImageBatchQuery expenseImageBatchQuery )
        {
            _logger = logger;
            _expenseImageBatchQuery = expenseImageBatchQuery;
        }

        public async Task<int> ProcessAsync(int batchSize = 0)
        {
            // 1) Get dataset of items that have no image set (batch size sensitive)
            _logger.LogInformation("Get dataset of items that have no image set (batch size sensitive) : {BatchSize}", batchSize);
            //ExpenseImageBatchQuery returns ExpenseID array
            var query = await _expenseImageBatchQuery.ExecuteAsync(batchSize);


            // 2) Download images  return an array of ExpenseIds and byte arrays.
            //Image downloader
            
            // 3) Asynchronous saving of array
            //return rows modifed
            //Expensse Batch Commaand
            

            return 0;
        }
    }
}

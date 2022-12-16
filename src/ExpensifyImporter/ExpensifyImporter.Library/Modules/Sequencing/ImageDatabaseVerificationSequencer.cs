using System;
using ExpensifyImporter.Library.Modules.Expensify;

namespace ExpensifyImporter.Library.Modules.Sequencing
{
    public class ImageDatabaseVerificationSequencer
    {
        private readonly ILogger<ImageToDatabaseSequencer> _logger;
        private readonly ExpenseImageBatchQuery _expenseImageBatchQuery;
        private readonly ExpensifyImageDownloader _expensifyImageDownloader;
        private readonly ExpenseImageBatchCommand _expenseImageBatchCommand;

        public ImageDatabaseVerificationSequencer(
            ILogger<ImageToDatabaseSequencer> logger,
            ExpenseImageBatchQuery expenseImageBatchQuery,
            ExpensifyImageDownloader expensifyImageDownloader,
            ExpenseImageBatchCommand expenseImageBatchCommand)
        {
            _logger = logger;
            _expenseImageBatchQuery = expenseImageBatchQuery;
            _expensifyImageDownloader = expensifyImageDownloader;
            _expenseImageBatchCommand = expenseImageBatchCommand;
        }

        public async Task<int> ProcessAsync(int batchSize = 0)
       { 
            // 1) Get dataset of items that have image set and are not verified (batch size sensitive)
            _logger.LogInformation("Get dataset of items that have image set and are not verified (batch size sensitive) : {BatchSize}", batchSize);
            //ExpenseImageBatchQuery returns ExpenseID array
            var query = await _expenseImageBatchQuery.ExecuteAsync(expense => expense.ReceiptImage != null && !expense.ImageVerified ,batchSize);


            // 2) Download images  return an array of ExpenseIds and byte arrays.
            _logger.LogInformation("Downloading image batch {BatchSize}", batchSize);
            var downloadResult = await _expensifyImageDownloader.ExecuteAsync(query);


            // 3) Verfy the downloaded images with those saved.
            _logger.LogInformation("Verfy the downloaded images with those saved. {BatchSize}", batchSize);



            return await Task.FromResult(0);
        }
    }
}


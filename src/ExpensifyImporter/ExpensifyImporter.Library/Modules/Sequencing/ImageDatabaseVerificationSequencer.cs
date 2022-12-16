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
            //Todo: add sequencing logic here.
            return await Task.FromResult(0);
        }
    }
}


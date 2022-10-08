using ExpensifyImporter.Database;
using ExpensifyImporter.Library.Modules.Expensify;

namespace ExpensifyImporter.Library.Modules.Database
{
    public class ExpenseImageBatchCommand
    {
        private readonly ILogger<ExpenseImageBatchCommand> _logger;
        private readonly ExpensifyContext _dbContext;

        public ExpenseImageBatchCommand(
            ILogger<ExpenseImageBatchCommand> logger,
            ExpensifyContext dbContext
        )
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<int> ExecuteAsync(List<ExpensifyImageDownloadResult> batch)
        {
            await Task.Delay(5);

            return 0;
        }
    }
}

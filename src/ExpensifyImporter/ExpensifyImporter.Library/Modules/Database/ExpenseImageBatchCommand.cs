using ExpensifyImporter.Database;
using ExpensifyImporter.Library.Modules.Expensify;
using Microsoft.EntityFrameworkCore;

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

        public async Task<int> ExecuteAsync(IEnumerable<ExpensifyImageDownloadResult> batch)
        {
            var tasks = batch.Select(UpdateExpenseByDownloadResult).ToList();

            var results = await Task.WhenAll(tasks);

            return results.Aggregate((a,b)=> a+b);
        }

        private async Task<int> UpdateExpenseByDownloadResult(ExpensifyImageDownloadResult downloadResult)
        {
            var result = await _dbContext.Expense.SingleOrDefaultAsync(s => s.Id == downloadResult.ExpenseId);

            if (result == null) return 0;
            result.ReceiptImage = downloadResult.FileContents;
            return await _dbContext.SaveChangesAsync();

        }
    }
}

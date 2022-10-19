using ExpensifyImporter.Database;
using ExpensifyImporter.Database.Domain;
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
            // var tasks = batch.Select(UpdateExpenseByDownloadResult).ToList();
            //
            // var results = await Task.WhenAll(tasks);
            foreach (var expensifyImageDownloadResult in batch)
            {
                var item = await _dbContext.Expense.SingleAsync(s => s.Id == expensifyImageDownloadResult.ExpenseId);
                item.ReceiptImage = expensifyImageDownloadResult.FileContents;
                _dbContext.Expense.Update(item);
            }

            //_dbContext.Expense.UpdateRange(results.ToList());

            return await _dbContext.SaveChangesAsync();
        }

        private async Task<Expense> UpdateExpenseByDownloadResult(ExpensifyImageDownloadResult downloadResult)
        {
            var result = await _dbContext.Expense.SingleAsync(s => s.Id == downloadResult.ExpenseId);

            result.ReceiptImage = downloadResult.FileContents;

            return result;
        }
    }
}

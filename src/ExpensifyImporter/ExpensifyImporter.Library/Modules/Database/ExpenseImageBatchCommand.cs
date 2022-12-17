using System.Collections.Concurrent;
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
        private  ConcurrentBag<Expense>? _expensesToUpdate;

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
            var batchList = batch.ToList();
            _expensesToUpdate = new ConcurrentBag<Expense>(await GetExpensesToUpdate(batchList));
           
            var tasks = batchList.Select(UpdateExpenseByDownloadResult).ToList();

            var updates = await Task.WhenAll(tasks);

            _dbContext.Expense.UpdateRange(updates);

            return await _dbContext.SaveChangesAsync();
        }

        private async Task<IEnumerable<Expense>> GetExpensesToUpdate(IEnumerable<ExpensifyImageDownloadResult> batch)
        {
            var batchIdCollection = batch.Select(s => s.Id);
            return await _dbContext.Expense.Where(w => batchIdCollection.Contains(w.Id)).ToListAsync();
        }

        private  Task<Expense> UpdateExpenseByDownloadResult(ExpensifyImageDownloadResult downloadResult)
        {
            
            var result = _expensesToUpdate?.Single(s => s.Id == downloadResult.Id);
            result!.ReceiptImage = downloadResult.FileContents;

            return Task.FromResult(result);
        }
    }
}

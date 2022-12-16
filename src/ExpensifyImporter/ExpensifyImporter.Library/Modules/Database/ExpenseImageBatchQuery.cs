using System.Linq.Expressions;

using ExpensifyImporter.Database;
using Microsoft.EntityFrameworkCore;

namespace ExpensifyImporter.Library.Modules.Database
{
    public record ExpenseImageBatchQueryResult(Guid ExpenseId,string? Url);
    public class ExpenseImageBatchQuery
    {
        private readonly ILogger<ExpenseImageBatchQuery> _logger;
        private readonly ExpensifyContext _dbContext;

        public ExpenseImageBatchQuery(ILogger<ExpenseImageBatchQuery> logger, ExpensifyContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<ExpenseImageBatchQueryResult>> ExecuteAsync(int batchSize = 0)
        {
            var queryable =  _dbContext.Expense.Where(expense => expense.ReceiptImage == null && expense.ReceiptUrl != null);

            if (batchSize > 0)
            {
                queryable = queryable.Take(batchSize);
            }

            return await queryable.Select(s=> new ExpenseImageBatchQueryResult(s.Id,s.ReceiptUrl)).ToListAsync();
        }
    }
}

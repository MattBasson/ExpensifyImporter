using System.Linq.Expressions;

using ExpensifyImporter.Database;
using ExpensifyImporter.Database.Domain;
using Microsoft.EntityFrameworkCore;

namespace ExpensifyImporter.Library.Modules.Database
{
    public record ExpenseImageBatchQueryResult(Guid ExpenseId,string? Url, byte[]? ReceiptImage);
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
            return await ExecuteAsync(expense => expense.ReceiptImage == null && expense.ReceiptUrl != null,batchSize);
        }

        public async Task<IEnumerable<ExpenseImageBatchQueryResult>> ExecuteAsync(Expression<Func<Expense,bool>> query, int batchSize = 0)
        {
            var queryable = _dbContext.Expense.Where(query);

            if (batchSize > 0)
            {
                queryable = queryable.Take(batchSize);
            }

            return await queryable.Select(s => new ExpenseImageBatchQueryResult(s.Id, s.ReceiptUrl,s.ReceiptImage)).ToListAsync();
        }
    }
}

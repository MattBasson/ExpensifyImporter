using System.Linq.Expressions;

using ExpensifyImporter.Database;
using ExpensifyImporter.Database.Domain;
using ExpensifyImporter.Library.Modules.Database.Domain;
using Microsoft.EntityFrameworkCore;

namespace ExpensifyImporter.Library.Modules.Database
{
    public record ExpenseImageBatchQueryResult(Guid Id,string? Url, byte[]? ReceiptImage) : EntityRecord(Id);
    public class ExpenseImageBatchQuery
    {
        private readonly ILogger<ExpenseImageBatchQuery> _logger;
        private readonly ExpensifyContext _dbContext;

        public ExpenseImageBatchQuery(ILogger<ExpenseImageBatchQuery> logger, ExpensifyContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }
        //Todo: Remove this function , iit can be replaced by sibling
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

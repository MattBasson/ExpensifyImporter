using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpensifyImporter.Database;
using ExpensifyImporter.Database.Domain;
using ExpensifyImporter.Library.Modules.Database.Domain;
using Microsoft.EntityFrameworkCore;

namespace ExpensifyImporter.Library.Modules.Database
{
    public class ExpenseImageBatchQuery
    {
        private readonly ILogger<ExpenseImageBatchQuery> _logger;
        private readonly ExpensifyContext _dbContext;

        public ExpenseImageBatchQuery(ILogger<ExpenseImageBatchQuery> logger, ExpensifyContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<ExpenseImage>> ExecuteAsync(int batchSize = 0)
        {
            var queryable =  _dbContext.Expense.Where(expense => expense.ReceiptImage == null);

            if (batchSize > 0)
            {
                queryable = queryable.Take(batchSize);
            }

            return await queryable.Select(s=> new ExpenseImage(s.Id,s.ReceiptUrl)).ToListAsync();
        }
    }
}

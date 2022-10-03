using ExpensifyImporter.Database;
using ExpensifyImporter.Database.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ExpensifyImporter.Library.Modules.Database
{
    public class ExpenseDuplicatesFilter
    {
        private readonly ILogger<ExpenseDuplicatesFilter> _logger;
        private readonly ExpensifyContext _dbContext;

        public ExpenseDuplicatesFilter(ILogger<ExpenseDuplicatesFilter> logger, ExpensifyContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<List<Expense>> Filter(IEnumerable<Expense> expenses)
        {
            var currentExpenses = await _dbContext.Expense.Select(s=>s.ReceiptId).ToListAsync();
        
            var currentExpensesHashSet = currentExpenses.ToHashSet();

            return expenses.Where(w => !currentExpensesHashSet.Contains(w.ReceiptId)).ToList();
        }
    }
}
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpensifyImporter.Library.Modules.Database
{
    public class ExpenseImageBatchQuery
    {
        private readonly ILogger<ExpenseImageBatchQuery> _logger;

        public ExpenseImageBatchQuery(ILogger<ExpenseImageBatchQuery> logger)
        {
            _logger = logger;
        }
    }
}

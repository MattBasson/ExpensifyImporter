using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpensifyImporter.Library.Modules.Database
{
    public class ExpenseImageBatchCommand
    {
        private readonly ILogger<ExpenseImageBatchCommand> _logger;

        public ExpenseImageBatchCommand(ILogger<ExpenseImageBatchCommand> logger)
        {
            _logger = logger;
        }
    }
}

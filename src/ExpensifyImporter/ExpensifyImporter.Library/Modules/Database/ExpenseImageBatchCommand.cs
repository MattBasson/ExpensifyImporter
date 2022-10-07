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

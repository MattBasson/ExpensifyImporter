using ExpensifyImporter.Database;
using ExpensifyImporter.Library.Modules.Excel;
using ExpensifyImporter.Library.Modules.Expensify;
using Microsoft.Extensions.Logging;

namespace ExpensifyImporter.Library.Modules.Sequencing
{
    public class ExcelToDatabaseSequencer
    {
        private readonly ILogger _logger;
        private readonly ExpensifyContext _dbContext;
        private readonly ExcelReader _excelReader;
        private readonly ExcelDtoMapper _excelDtoMapper;
        private readonly ExpensifyModelExcelDtoMapper _expensifyModelExcelDtoMapper;

        public ExcelToDatabaseSequencer(ILogger<ExcelToDatabaseSequencer> logger,
            ExpensifyContext dbContext,
            ExcelReader excelReader,
            ExcelDtoMapper excelDtoMapper,
            ExpensifyModelExcelDtoMapper expensifyModelExcelDtoMapper)
        {
            _logger = logger; 
            _dbContext = dbContext;
            _excelReader = excelReader;
            _excelDtoMapper = excelDtoMapper;
            _expensifyModelExcelDtoMapper = expensifyModelExcelDtoMapper;
        }

        public async Task<int> ProcessDocumentAsync(string path)
        {
            // 1) will read the file to get the Json.
            _logger.LogInformation("Reading Excel Document and parsing to Json from here : {Path}", path);
            var excelJson = await _excelReader.ReadAsJsonAsync(path);

            // 2) Will deserialize that the json into a generic excel Poco
            _logger.LogInformation("Deserializing Json to Excel Poco for Json : {ExcelJson}", excelJson);
            var excelSheets = await _excelDtoMapper.DeserializeAsync(excelJson);
        
            // 3) Will map the excel poco to an expense model collection
            _logger.LogInformation("Mapping to expense collection for this amount of sheets : {ExcelSheetsCount} ", excelSheets.Count);
            var expenses = await _expensifyModelExcelDtoMapper.MapExpensesAsync(excelSheets);

            // 4) Will save the expense model collection to the database.
            _logger.LogInformation("Saving expenses to database for {ExpensesCount} items", expenses.Count);
            await _dbContext.Expense.AddRangeAsync(expenses);
            return await _dbContext.SaveChangesAsync();
        }
    }
}
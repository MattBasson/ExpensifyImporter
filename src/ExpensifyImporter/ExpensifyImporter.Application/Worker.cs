using ExpensifyImporter.Library.Modules.IO;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ExpensifyImporter.Application
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly ExcelFileWatcher _excelFileWatcher;

        public Worker(ILogger<Worker> logger, ExcelFileWatcher excelFileWatcher)
        {
            _logger = logger;
            _excelFileWatcher = excelFileWatcher;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                //_logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}

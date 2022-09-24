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
            _excelFileWatcher.Created += _excelFileWatcher_Created;
        }

        private void _excelFileWatcher_Created(object sender, FileSystemEventArgs e)
        {
            //Functionality to read directory pathg
            throw new NotImplementedException();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                //_logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);


                var path = _excelFileWatcher.Path;

                await Task.Delay(60000, stoppingToken);
            }
        }
    }
}

using ExpensifyImporter.Library.Domain;
using ExpensifyImporter.Library.Modules.IO;
using ExpensifyImporter.Library.Modules.Sequencing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ExpensifyImporter.Application
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly WorkerConfiguration _workerConfiguration;
        private readonly FeatureFlagsConfiguration _featureConfiguration;
        private readonly ExcelToDatabaseSequencer _excelSequencer;
        private readonly DirectoryInfo _directoryInfo;
        private readonly string _watchPath;

        public Worker(
            ILogger<Worker> logger,
            IOptions<WorkerConfiguration> workerConfiguration,
            IOptions<FeatureFlagsConfiguration> featureConfiguration,
            ExcelFileWatcher excelFileWatcher,
            ExcelToDatabaseSequencer excelSequencer)
        {
            _logger = logger;
            _workerConfiguration = workerConfiguration.Value;
            _featureConfiguration = featureConfiguration.Value;
            _excelSequencer = excelSequencer;
            _watchPath = excelFileWatcher.Path;
            if (_featureConfiguration.WatchDirectory)
            {
                _logger.LogInformation("Watch directory enabled listening file additions in path : {WatchPath}", _watchPath);
                excelFileWatcher.Created += _excelFileWatcher_Created;
            }            
            _directoryInfo = new DirectoryInfo(excelFileWatcher.Path);
        }

        private void _excelFileWatcher_Created(object sender, FileSystemEventArgs e)
        {
            Task.Run(async () => await ProcessExcelFile(e.FullPath));
         }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogDebug("Debug : Watch directory enabled : {WatchDirectory}", _featureConfiguration.WatchDirectory);
            _logger.LogDebug("Debug : Poll directory enabled : {PollDirectory}", _featureConfiguration.PollDirectory);
            
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {Time}", DateTimeOffset.Now);

                if (_featureConfiguration.PollDirectory)
                {
                    _logger.LogInformation("Poll directory enabled running ProcessExcelFiles for path: {WatchPath}", _watchPath);
                    await ProcessExcelFilesInPath();
                }
                await Task.Delay(_workerConfiguration.Interval, stoppingToken);
            }
        }

        private async Task ProcessExcelFilesInPath()
        {
            var files = _directoryInfo.GetFiles("*.xlsx");

            if(files.Any())
            {
                _logger.LogInformation("Processing excel files, found {Count}", files.Length);
                foreach (var file in files)
                {
                    await ProcessExcelFile(file.FullName);
                }
            }
        }

        private async Task ProcessExcelFile(string path)
        {
            _logger.LogInformation("Processing excel file: {Path}", path);
            var result = await _excelSequencer.ProcessDocumentAsync(path);
            if (result > 0)
            {
                _logger.LogInformation("Processing excel file complete and saved: {Path}", path);
                File.Delete(path);
            }
        }
    }
}

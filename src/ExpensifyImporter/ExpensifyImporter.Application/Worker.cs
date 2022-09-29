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
            if (_featureConfiguration.WatchDirectory)
            {
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
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                if (_featureConfiguration.PollDirectory)
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                    await ProcessExcelFilesInPath();
                }
                

                await Task.Delay(_workerConfiguration.Interval, stoppingToken);
            }
        }

        private async Task ProcessExcelFilesInPath()
        {
            var files = _directoryInfo.GetFiles("*.xlsx");                      

            if(files != null && files.Any())
            {
                _logger.LogInformation($"Procesing excel files, found {files.Count()}");
                foreach (var file in files)
                {
                    await ProcessExcelFile(file.FullName);
                }
            }
        }

        private async Task ProcessExcelFile(string path)
        {
            _logger.LogInformation($"Procesing excel file: {path}");
            var result = await _excelSequencer.ProcessDocumentAsync(path);
            if (result > 0)
            {
                _logger.LogInformation($"Procesing excel file complete and saved: {path}");
                File.Delete(path);
            }
        }
    }
}

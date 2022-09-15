using Microsoft.Extensions.FileSystemGlobbing;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpensifyImporter.Library.Modules.Io
{
    public class ExcelFileWatcher
    {
        private readonly ILogger<ExcelFileWatcher> _logger;
        private readonly FileSystemWatcher _fileSystemWatcher;


        public ExcelFileWatcher(ILogger<ExcelFileWatcher> logger, string path)
        {
            _logger = logger;
            _logger.LogInformation($"Starting file watcher for this watchPath : {path}");

            _fileSystemWatcher = new FileSystemWatcher(path);

            //_fileSystemWatcher.NotifyFilter = NotifyFilters.Attributes
            //                     | NotifyFilters.CreationTime
            //                     | NotifyFilters.DirectoryName
            //                     | NotifyFilters.FileName
            //                     | NotifyFilters.LastAccess
            //                     | NotifyFilters.LastWrite
            //                     | NotifyFilters.Security
            //                     | NotifyFilters.Size;

            _fileSystemWatcher.Filter = "*.xlsx";
            _fileSystemWatcher.IncludeSubdirectories = true;
            _fileSystemWatcher.EnableRaisingEvents = true;

            _fileSystemWatcher.Changed += FileSystemWatcher_Changed;
            _fileSystemWatcher.Created += FileSystemWatcher_Created;
            _fileSystemWatcher.Deleted += FileSystemWatcher_Deleted;
            _fileSystemWatcher.Renamed += FileSystemWatcher_Renamed;
            _fileSystemWatcher.Error += FileSystemWatcher_Error;

        }

        private void FileSystemWatcher_Error(object sender, ErrorEventArgs e)
        {
            var exception = e.GetException();
            _logger.LogError(exception, exception.Message );
        }

        private void FileSystemWatcher_Renamed(object sender, RenamedEventArgs e)
        {
            _logger.LogInformation($"Renamed:");
            _logger.LogInformation($"       Old: {e.OldFullPath}");
            _logger.LogInformation($"       New: {e.FullPath}");
        }

        private void FileSystemWatcher_Deleted(object sender, FileSystemEventArgs e)
        {
            _logger.LogInformation($"Deleted: {e.FullPath}");
        }

        private void FileSystemWatcher_Created(object sender, FileSystemEventArgs e)
        {
            _logger.LogInformation($"Created: {e.FullPath}");
        }

        private void FileSystemWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Changed)
            {
                return;
            }
            _logger.LogInformation($"Changed: {e.FullPath}");
        }
    }
}

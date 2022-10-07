using Microsoft.Extensions.FileSystemGlobbing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpensifyImporter.Library.Modules.IO
{
    public class ExcelFileWatcher:FileSystemWatcher
    {
        private readonly ILogger<ExcelFileWatcher> _logger;        



        public ExcelFileWatcher(ILogger<ExcelFileWatcher> logger, string path):base(path)
        {
            _logger = logger;
            _logger.LogInformation($"Starting file watcher for this watchPath : {path}");

            //this.NotifyFilter = NotifyFilters.Attributes
            //                     | NotifyFilters.CreationTime
            //                     | NotifyFilters.DirectoryName
            //                     | NotifyFilters.FileName
            //                     | NotifyFilters.LastAccess
            //                     | NotifyFilters.LastWrite
            //                     | NotifyFilters.Security
            //                     | NotifyFilters.Size;

            this.Filter = "*.xlsx";
            this.IncludeSubdirectories = true;
            this.EnableRaisingEvents = true;

            this.Changed += FileSystemWatcher_Changed;
            this.Created += FileSystemWatcher_Created;
            this.Deleted += FileSystemWatcher_Deleted;
            this.Renamed += FileSystemWatcher_Renamed;
            this.Error += FileSystemWatcher_Error;

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

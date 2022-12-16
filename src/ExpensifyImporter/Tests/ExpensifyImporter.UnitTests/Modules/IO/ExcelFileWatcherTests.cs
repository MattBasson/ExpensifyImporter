using Castle.Core.Logging;
using ExpensifyImporter.Library.Modules.IO;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ExpensifyImporter.UnitTests.Modules.IO
{
    public class ExcelFileWatcherTests : IDisposable
    {
        
        private readonly string _testWatchDirectory = $"{Environment.CurrentDirectory}{Path.DirectorySeparatorChar}TestWatchDir{Path.DirectorySeparatorChar}";
        private readonly ExcelFileWatcher _fileWatcher;

        public ExcelFileWatcherTests()
        {
            Directory.CreateDirectory(_testWatchDirectory);
            _fileWatcher = new ExcelFileWatcher(Substitute.For<ILogger<ExcelFileWatcher>>(), _testWatchDirectory);
        }

        [Fact]
        public void Create_Event_Raised_When_File_Added_To_Watch_Directory()
        {
            //Arrange            
            //Monitor for events from Excel File watcher./
            using var excelFileWatcherMonitor = _fileWatcher.Monitor();

           
            File.WriteAllText($"{_testWatchDirectory}testFile.xlsx","Test Test Test");
            
            //Sleeping for a second to let the events register.
            Thread.Sleep(1000);

            excelFileWatcherMonitor.Should().Raise("Created");
            excelFileWatcherMonitor.Should().Raise("Created")
                .WithSender(_fileWatcher)
                .WithArgs<FileSystemEventArgs>(args => args.FullPath.Contains(_testWatchDirectory));


        }


        [Fact]
        public void Create_Event_Not_Raised_When_File_Added_To_Watch_Directory_Invalid_FileType()
        {
            //Arrange            
            //Monitor for events from Excel File watcher./
            using var excelFileWatcherMonitor = _fileWatcher.Monitor();


            File.WriteAllText($"{_testWatchDirectory}testFile.xls", "Test Test Test");
            File.WriteAllText($"{_testWatchDirectory}testFile.txt", "Test Test Test");
            File.WriteAllText($"{_testWatchDirectory}testFile.png", "Test Test Test");
            File.WriteAllText($"{_testWatchDirectory}testFile.docx", "Test Test Test");

            //Sleeping for a second to let the events register.
            Thread.Sleep(1000);

            excelFileWatcherMonitor.Should().NotRaise("Created");           

        }

        public void Dispose()
        {            
            _fileWatcher.Dispose();
            Directory.Delete(_testWatchDirectory, true);
        }       

    }
}

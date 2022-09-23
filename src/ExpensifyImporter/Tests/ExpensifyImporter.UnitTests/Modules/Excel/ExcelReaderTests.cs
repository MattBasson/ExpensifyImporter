using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpensifyImporter.UnitTests.Modules.Excel
{

    public class ExcelReaderTests
    {
        [Fact]
        public void Reading_Excel_Test_File_Exists()
        {
            //Arrange
            var path = $"{Environment.CurrentDirectory}\\Modules\\Excel\\Data\\TestExcel.xlsx";

            //Act

            //Assert
            File.Exists(path).Should().BeTrue();

            path.Should().Contain("UnitTests");
        }
    }
}
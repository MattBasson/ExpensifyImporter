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
        public void Reading_Excel_Returns_Correct_Data()
        {
            //Arrange
            var path = $"{Environment.CurrentDirectory}\\Modules\\Excel\\Data\\TestExcel.xlsx";

            File.Exists(path).Should().BeTrue();

            path.Should().Contain("UnitTests");
        }
    }
}
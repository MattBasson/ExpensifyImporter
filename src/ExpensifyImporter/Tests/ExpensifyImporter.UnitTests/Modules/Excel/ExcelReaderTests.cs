using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ExpensifyImporter.Library.Modules.Excel;

using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

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

        [Fact]
        public void Reading_Excel_File_Yields_Results()
        {
            //Arrange
            var path = $"{Environment.CurrentDirectory}\\Modules\\Excel\\Data\\TestExcel.xlsx";
            var excelReader = new ExcelReader(Substitute.For<ILogger<ExcelReader>>());

            var excelResponse = excelReader.ReadAsJson(path);

            excelResponse.Should().Contain("Matt");
            excelResponse.Should().Contain("Tess");
            excelResponse.Should().Contain("Barry");

        }
        
        
        [Fact]
        public async Task Reading_Excel_File_Yields_Results_In_Correct_Structure()
        {
            //Arrange
            var path = $"{Environment.CurrentDirectory}\\Modules\\Excel\\Data\\TestExcel.xlsx";
            var excelReader = new ExcelReader(Substitute.For<ILogger<ExcelReader>>());

            var excelResponse = excelReader.ReadAsJson(path);

            using var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(excelResponse));
            var jsonDocument = await JsonDocument.ParseAsync(memoryStream);

            jsonDocument.RootElement.Should().BeNull();

        }
    }
}
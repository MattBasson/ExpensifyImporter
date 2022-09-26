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
        public async Task Reading_Excel_File_Yields_Results()
        {
            //Arrange
            var path = $"{Environment.CurrentDirectory}\\Modules\\Excel\\Data\\TestExcel.xlsx";
            var excelReader = new ExcelReader(Substitute.For<ILogger<ExcelReader>>());

            var excelResponse = await excelReader.ReadAsJsonAsync(path);

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

            var expectedResponseDEserialised = new List<List<string[]>>();
            expectedResponseDEserialised.Add(new List<string[]>() {
                new[] { "Id", "Name","Salary"},
                new[] { "8BC78143-9FD5-45E4-AEED-F5648D58473C", "Matt","1000"},
                new[] { "46C6F115-B719-48BF-8EE1-3ABF480DF748", "Tess","1200"},
                new[] { "5088AB6B-CFCE-4531-BDFE-1E79CCAA7A3D", "Barry","1300"}
            });


            //Act
            var excelResponse = await excelReader.ReadAsJsonAsync(path);

            var excelResponseDeserialised = JsonSerializer.Deserialize<List<List<string[]>>>(excelResponse);


            //Assert

            excelResponseDeserialised.Should().BeEquivalentTo(excelResponseDeserialised);
        }
    }
}
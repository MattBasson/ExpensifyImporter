using Castle.Core.Logging;
using ExpensifyImporter.Library.Modules.Excel;
using ExpensifyImporter.Library.Modules.Excel.Domain;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpensifyImporter.UnitTests.Modules.Excel
{
    public class ExcelDtoMapperTests
    {

        [Fact]
        public async Task Mapping_Excel_Json_Yields_Correct_POCO_List()
        {
            //Arrange
            var excelJson = "[[[[\"A1\",\"Id\"],[\"B1\",\"Name\"],[\"C1\",\"Salary\"]],[[\"A2\",\"8BC78143-9FD5-45E4-AEED-F5648D58473C\"],[\"B2\",\"Matt\"],[\"C2\",\"1000\"]],[[\"A3\",\"46C6F115-B719-48BF-8EE1-3ABF480DF748\"],[\"B3\",\"Tess\"],[\"C3\",\"1200\"]],[[\"A4\",\"5088AB6B-CFCE-4531-BDFE-1E79CCAA7A3D\"],[\"B4\",\"Barry\"],[\"C4\",\"1300\"]]]]";
            var mapper = new ExcelDtoMapper(Substitute.For<ILogger<ExcelDtoMapper>>());

            var expectedPocoList = new List<ExcelSheet>
            {
                new()
                {
                    new ExcelRow
                    {
                        new( "Id", "A1", "A"),
                        new( "Name", "B1","B"),
                        new( "Salary", "C1", "C")
                    },
                    new ExcelRow
                    {
                        new( "8BC78143-9FD5-45E4-AEED-F5648D58473C", "A2", "A"),
                        new( "Matt", "B2", "B"),
                        new( "1000", "C2", "C")
                    },
                    new ExcelRow
                    {
                        new( "46C6F115-B719-48BF-8EE1-3ABF480DF748", "A3", "A"),
                        new( "Tess", "B3", "B"),
                        new( "1200", "C3", "C")
                    },
                    new ExcelRow
                    {
                        new( "5088AB6B-CFCE-4531-BDFE-1E79CCAA7A3D","A4", "A"),
                        new( "Barry", "B4", "B"),
                        new( "1300", "C4", "C")
                    }
                }
            };

            //Act
            var excelPocoList = await mapper.DeserializeAsync(excelJson,false);

            //Assert
            excelPocoList.Should().BeEquivalentTo(expectedPocoList);
        }

        [Fact]
        public async Task Mapping_Excel_Json_Yields_Correct_POCO_List_Excluding_Headers()
        {
            //Arrange
            var excelJson = "[[[[\"A1\",\"Id\"],[\"B1\",\"Name\"],[\"C1\",\"Salary\"]],[[\"A2\",\"8BC78143-9FD5-45E4-AEED-F5648D58473C\"],[\"B2\",\"Matt\"],[\"C2\",\"1000\"]],[[\"A3\",\"46C6F115-B719-48BF-8EE1-3ABF480DF748\"],[\"B3\",\"Tess\"],[\"C3\",\"1200\"]],[[\"A4\",\"5088AB6B-CFCE-4531-BDFE-1E79CCAA7A3D\"],[\"B4\",\"Barry\"],[\"C4\",\"1300\"]]]]";
            var mapper = new ExcelDtoMapper(Substitute.For<ILogger<ExcelDtoMapper>>());

            var expectedPocoList = new List<ExcelSheet>
            {
                new()
                {                  
                    new ExcelRow
                    {
                        new( "8BC78143-9FD5-45E4-AEED-F5648D58473C", "A2", "A"),
                        new( "Matt", "B2", "B"),
                        new( "1000", "C2", "C")
                    },
                    new ExcelRow
                    {
                        new("46C6F115-B719-48BF-8EE1-3ABF480DF748", "A3", "A"),
                        new( "Tess", "B3", "B"),
                        new( "1200", "C3", "C")
                    },
                    new ExcelRow
                    {
                        new("5088AB6B-CFCE-4531-BDFE-1E79CCAA7A3D","A4", "A"),
                        new("Barry", "B4", "B"),
                        new("1300", "C4", "C")
                    }
                }
            };

            //Act
            var excelPocoList = await mapper.DeserializeAsync(excelJson);

            //Assert
            excelPocoList.Should().BeEquivalentTo(expectedPocoList);
        }
    }
}

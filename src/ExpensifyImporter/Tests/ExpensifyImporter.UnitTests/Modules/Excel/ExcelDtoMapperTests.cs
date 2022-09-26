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
            var excelJson = "[[[\"Id\",\"Name\",\"Salary\"],[\"8BC78143-9FD5-45E4-AEED-F5648D58473C\",\"Matt\",\"1000\"],[\"46C6F115-B719-48BF-8EE1-3ABF480DF748\",\"Tess\",\"1200\"],[\"5088AB6B-CFCE-4531-BDFE-1E79CCAA7A3D\",\"Barry\",\"1300\"]]]";
            var mapper = new ExcelDtoMapper(Substitute.For<ILogger<ExcelDtoMapper>>());

            var expectedPocoList = new List<ExcelSheet>
            {
                new()
                {
                    new ExcelRow
                    {
                        new(0, "Id"),
                        new(1, "Name"),
                        new(2, "Salary")
                    },
                    new ExcelRow
                    {
                        new(0, "8BC78143-9FD5-45E4-AEED-F5648D58473C"),
                        new(1, "Matt"),
                        new(2, "1000")
                    },
                    new ExcelRow
                    {
                        new(0, "46C6F115-B719-48BF-8EE1-3ABF480DF748"),
                        new(1, "Tess"),
                        new(2, "1200")
                    },
                    new ExcelRow
                    {
                        new(0, "5088AB6B-CFCE-4531-BDFE-1E79CCAA7A3D"),
                        new(1, "Barry"),
                        new(2, "1300")
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
            var excelJson = "[[[\"Id\",\"Name\",\"Salary\"],[\"8BC78143-9FD5-45E4-AEED-F5648D58473C\",\"Matt\",\"1000\"],[\"46C6F115-B719-48BF-8EE1-3ABF480DF748\",\"Tess\",\"1200\"],[\"5088AB6B-CFCE-4531-BDFE-1E79CCAA7A3D\",\"Barry\",\"1300\"]]]";
            var mapper = new ExcelDtoMapper(Substitute.For<ILogger<ExcelDtoMapper>>());

            var expectedPocoList = new List<ExcelSheet>
            {
                new()
                {
                    new ExcelRow
                    {
                        new(0, "8BC78143-9FD5-45E4-AEED-F5648D58473C"),
                        new(1, "Matt"),
                        new(2, "1000")
                    },
                    new ExcelRow
                    {
                        new(0, "46C6F115-B719-48BF-8EE1-3ABF480DF748"),
                        new(1, "Tess"),
                        new(2, "1200")
                    },
                    new ExcelRow
                    {
                        new(0, "5088AB6B-CFCE-4531-BDFE-1E79CCAA7A3D"),
                        new(1, "Barry"),
                        new(2, "1300")
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

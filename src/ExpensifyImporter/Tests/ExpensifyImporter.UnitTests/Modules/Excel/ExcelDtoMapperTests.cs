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
        public void Mapping_Excel_Json_Yields_Correct_POCO_List()
        {
            //Arrange
            var excelJson = "[[[\"Id\",\"Name\",\"Salary\"],[\"8BC78143-9FD5-45E4-AEED-F5648D58473C\",\"Matt\",\"1000\"],[\"46C6F115-B719-48BF-8EE1-3ABF480DF748\",\"Tess\",\"1200\"],[\"5088AB6B-CFCE-4531-BDFE-1E79CCAA7A3D\",\"Barry\",\"1300\"]]]";
            var mapper = new ExcelDtoMapper(Substitute.For<ILogger<ExcelDtoMapper>>());
          
            var expectedPocoList = new List<ExcelSheet>()
            {
                new ExcelSheet()
                {
                    new ExcelRow()
                    {
                        new ExcelCell("Id"),
                        new ExcelCell("Name"),
                        new ExcelCell("Salary"),
                    },
                    new ExcelRow()
                    {
                        new ExcelCell("8BC78143-9FD5-45E4-AEED-F5648D58473C"),
                        new ExcelCell("Matt"),
                        new ExcelCell("1000"),
                    },
                    new ExcelRow()
                    {
                        new ExcelCell("46C6F115-B719-48BF-8EE1-3ABF480DF748"),
                        new ExcelCell("Tess"),
                        new ExcelCell("1200"),
                    },
                    new ExcelRow()
                    {
                        new ExcelCell("5088AB6B-CFCE-4531-BDFE-1E79CCAA7A3D"),
                        new ExcelCell("Barry"),
                        new ExcelCell("1300"),
                    }

                }
            };

            //Act
            var excelPocoList = mapper.Deserialize(excelJson,false);

            //Assert
            excelPocoList.Should().BeEquivalentTo(expectedPocoList);
        }

        [Fact]
        public void Mapping_Excel_Json_Yields_Correct_POCO_List_Excluding_Headers()
        {
            //Arrange
            var excelJson = "[[[\"Id\",\"Name\",\"Salary\"],[\"8BC78143-9FD5-45E4-AEED-F5648D58473C\",\"Matt\",\"1000\"],[\"46C6F115-B719-48BF-8EE1-3ABF480DF748\",\"Tess\",\"1200\"],[\"5088AB6B-CFCE-4531-BDFE-1E79CCAA7A3D\",\"Barry\",\"1300\"]]]";
            var mapper = new ExcelDtoMapper(Substitute.For<ILogger<ExcelDtoMapper>>());

            var expectedPocoList = new List<ExcelSheet>()
            {
                new ExcelSheet()
                {                   
                    new ExcelRow()
                    {
                        new ExcelCell("8BC78143-9FD5-45E4-AEED-F5648D58473C"),
                        new ExcelCell("Matt"),
                        new ExcelCell("1000"),
                    },
                    new ExcelRow()
                    {
                        new ExcelCell("46C6F115-B719-48BF-8EE1-3ABF480DF748"),
                        new ExcelCell("Tess"),
                        new ExcelCell("1200"),
                    },
                    new ExcelRow()
                    {
                        new ExcelCell("5088AB6B-CFCE-4531-BDFE-1E79CCAA7A3D"),
                        new ExcelCell("Barry"),
                        new ExcelCell("1300"),
                    }

                }
            };

            //Act
            var excelPocoList = mapper.Deserialize(excelJson);

            //Assert
            excelPocoList.Should().BeEquivalentTo(expectedPocoList);
        }
    }
}

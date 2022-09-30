using ExpensifyImporter.Library.Modules.IO;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ExpensifyImporter.UnitTests.Modules.IO
{
    public class EmbeddedDataTests
    {

        

        [Fact]
        public void Getting_Embedded_Data_Yields_Correct_File_And_Content_With_Declared_Assembly()
        {
            //Arrange
            var assembly = Assembly.GetAssembly(typeof(EmbeddedDataTests));
            //Act
            string fileContent = EmbeddedData.Get("ExpensifyImporter.UnitTests.Modules.IO.Data.TestContent.txt", assembly);

            //Assert
            fileContent.Should().NotBeNull();
            fileContent.Should().Contain("This is a test.");

        }
    }
}

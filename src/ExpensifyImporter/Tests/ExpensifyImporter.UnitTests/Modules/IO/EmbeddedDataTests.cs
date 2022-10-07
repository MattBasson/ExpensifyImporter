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

        [Theory]
        [InlineData("ExpensifyImporter.UnitTests.Modules.IO.Data.TestContent.txt")]
        [InlineData("ExpensifyImporter.UnitTests.Modules.IO.Data.cat_cats_eyes_cat_face_269574.webp")]
        [InlineData("ExpensifyImporter.UnitTests.Modules.IO.Data.cat_domestic_cat_sweet_269854.webp")]
        [InlineData("ExpensifyImporter.UnitTests.Modules.IO.Data.cat_feline_cats_eye_220526.webp")]
        public void Getting_Embedded_Data_Yields_Correct_File_And_ContentString_With_Declared_Assembly(string path)
        {
            //Arrange
            var assembly = Assembly.GetAssembly(typeof(EmbeddedDataTests));
            //Act
            string fileContent = EmbeddedData.Get(path, assembly);

            //Assert
            fileContent.Should().NotBeNull();
            fileContent.Length.Should().BePositive();

        }
        
        [Theory]
        [InlineData("ExpensifyImporter.UnitTests.Modules.IO.Data.TestContent.txt")]
        [InlineData("ExpensifyImporter.UnitTests.Modules.IO.Data.cat_cats_eyes_cat_face_269574.webp")]
        [InlineData("ExpensifyImporter.UnitTests.Modules.IO.Data.cat_domestic_cat_sweet_269854.webp")]
        [InlineData("ExpensifyImporter.UnitTests.Modules.IO.Data.cat_feline_cats_eye_220526.webp")]
        public void Getting_Embedded_Data_Yields_Correct_File_And_ContentByteArray_With_Declared_Assembly(string path)
        {
            //Arrange
            var assembly = Assembly.GetAssembly(typeof(EmbeddedDataTests));
            //Act
            var fileContent = EmbeddedData.GetByteArray(path, assembly);

            //Assert
            fileContent.Should().NotBeNull();
            fileContent.Length.Should().BePositive();

        }
        
        
    }
}

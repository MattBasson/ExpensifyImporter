
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Wordprocessing;
using ExpensifyImporter.Library.Modules.Flags;
using ExpensifyImporter.Library.Modules.Flags.Domain;
using ExpensifyImporter.Library.Modules.IO;
using FluentAssertions;
using System.IO;

namespace ExpensifyImporter.UnitTests.Modules.Flags
{
    public class FlagFactoryTests
    {      

        [Fact]
        public void Providing_Unsupported_Flags_Yields_No_Results()
        {
            //Arrange
            var args = new string[]
            {
                "--path",
                "C:\\Dev\\Tools\\ImporterLocation",
                "--option",
                "fafa"
            };

            var flagFactory = new FlagFactory(args);           

            //Act
            var flags = flagFactory.GetSupportedFlags();

            //Assert
            flags.Should().BeEmpty();

        }


        [Fact]
        public void Providing_Help_Flag_Yields_Help_Flag()
        {
            //Arrange
            var args = new string[]
            {                
                "--help",                                
            };

            var flagFactory = new FlagFactory(args);

            //Act
            var flags = flagFactory.GetSupportedFlags();

            //Assert
            flags.Should().NotBeEmpty();
            var flag = flags.First();
            flag.Flag.Should().Be(FlagType.Help);
            flag.FlagValue.Should().Be("--help");
        }

        [Fact]
        public void Providing_Directory_Flag_Yields_Directory_Flag()
        {
            //Arrange
            var args = new string[]
            {
                "--directory",
                "C:\\Dev\\Tools\\ImporterLocation"
            };

            var flagFactory = new FlagFactory(args);

            //Act
            var flags = flagFactory.GetSupportedFlags();

            //Assert
            flags.Should().NotBeEmpty();

            var flag = flags.First();
            flag.Flag.Should().Be(FlagType.Directory);
            flag.FlagValue.Should().Be("C:\\Dev\\Tools\\ImporterLocation");
        }

        

    }
}
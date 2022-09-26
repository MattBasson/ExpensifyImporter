using ExpensifyImporter.Library.Modules.Excel;
using ExpensifyImporter.Library.Modules.Expensify;
using ExpensifyImporter.Library.Modules.Sequencing;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace ExpensifyImporter.UnitTests.Modules.Sequencing;

public class ExcelToDatabaseSequencerTests
{
    [Fact]
    public void Reading_Excel_Test_File_Exists()
    {
        //Arrange
        var path = $"{Environment.CurrentDirectory}\\Modules\\Sequencing\\Data\\Expense_Batch_Test_2017_02_01_1.xlsx";

        //Act

        //Assert
        File.Exists(path).Should().BeTrue();

        path.Should().Contain("UnitTests");
    }
    
    [Fact]
    public async Task When_ProcessDocumentAsync_Ensure_All_Items_Are_Added_To_DB()
    {
        //Arrange
        var path = $"{Environment.CurrentDirectory}\\Modules\\Sequencing\\Data\\Expense_Batch_Test_2017_02_01_1.xlsx";
        var dbContext = SQLiteHelper.CreateSqliteContext();
        var excelReader = new ExcelReader(Substitute.For<ILogger<ExcelReader>>());
        var excelDtoMapper = new ExcelDtoMapper(Substitute.For<ILogger<ExcelDtoMapper>>());
        var expensifyMapper = new ExpensifyModelExcelDtoMapper(Substitute.For<ILogger<ExpensifyModelExcelDtoMapper>>());
        var sequencer = new ExcelToDatabaseSequencer(
            Substitute.For<ILogger<ExcelToDatabaseSequencer>>(),
            dbContext,
            excelReader,
            excelDtoMapper,
            expensifyMapper);

        //Act 
        var dbResponse = await sequencer.ProcessDocumentAsync(path);

        //Assert
        dbContext.Expense.Count().Should().Be(9);
    }
}
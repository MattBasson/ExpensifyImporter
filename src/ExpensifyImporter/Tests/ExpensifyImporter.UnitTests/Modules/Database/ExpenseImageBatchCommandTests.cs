using ExpensifyImporter.Library.Modules.Database;
using ExpensifyImporter.Library.Modules.Expensify;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace ExpensifyImporter.UnitTests.Modules.Database
{
    public class ExpenseImageBatchCommandTests
    {
        public ExpenseImageBatchCommandTests()
        {
        }

        [Fact]
        public async Task Given_Batch_Of_Download_Results_Is_Saved_To_Database_Successfully()
        {
            //Arrange
            var dbContext = SQLiteHelper.CreateSqliteContext();
            
            var sut = new ExpenseImageBatchCommand(
                Substitute.For<ILogger<ExpenseImageBatchCommand>>(),
                dbContext);
            
            //Act
            var result = await sut.ExecuteAsync(new List<ExpensifyImageDownloadResult>());


            //Assert
            result.Should().NotBe(0);
        }
    }
}
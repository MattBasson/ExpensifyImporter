using System;
using System.Net.Http;
using System.Reflection;
using ExpensifyImporter.Library.Modules.Database;
using ExpensifyImporter.Library.Modules.Expensify;
using ExpensifyImporter.Library.Modules.IO;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace ExpensifyImporter.UnitTests.Modules.IO
{
	public class ExpenseImageVerifierTests
	{
        private readonly ExpenseImageVerifier _sut;

        private readonly Guid _item1Id = Guid.NewGuid();
        private readonly Guid _item2Id = Guid.NewGuid();
        private readonly Guid _item3Id = Guid.NewGuid();

        private byte[] _catImage1;
        private byte[] _catImage2;
        private byte[] _catImage3;

        public ExpenseImageVerifierTests()
        {
            _sut = new ExpenseImageVerifier();
            
        }

        [Fact]
        public async Task Expense_Images_Should_Match_DownloadedImages()
        {
            //Arrange
            _catImage1 = await
                EmbeddedData.GetByteArrayAsync($"ExpensifyImporter.UnitTests.Modules.IO.Data.{Constants.CatImageFile1}",
                    Assembly.GetAssembly(typeof(ExpenseImageVerifierTests)));

            _catImage2 = await
               EmbeddedData.GetByteArrayAsync($"ExpensifyImporter.UnitTests.Modules.IO.Data.{Constants.CatImageFile2}",
                   Assembly.GetAssembly(typeof(ExpenseImageVerifierTests)));

            _catImage3 = await
               EmbeddedData.GetByteArrayAsync($"ExpensifyImporter.UnitTests.Modules.IO.Data.{Constants.CatImageFile3}",
                   Assembly.GetAssembly(typeof(ExpenseImageVerifierTests)));


            var queryResult = new List<ExpenseImageBatchQueryResult>
            {
                new(_item1Id,Constants.CatImageUrl1,_catImage1),
                new(_item2Id,Constants.CatImageUrl2,_catImage2),
                new(_item3Id,Constants.CatImageUrl3,_catImage3)
            };

            var downloadResult = new List<ExpensifyImageDownloadResult>
            {
                new(_item1Id,_catImage1),
                new(_item2Id,_catImage2),
                new(_item3Id,_catImage3)
            };

            var expectedResult = new List<ExpenseImageVerificationResult>
            {
                new(_item1Id,true,DateTime.UtcNow),
                new(_item2Id,true,DateTime.UtcNow),
                new(_item3Id,true,DateTime.UtcNow),
            };

            //Act 
            var result = _sut.Execute(queryResult, downloadResult);


            //Assert
            result.Should().BeEquivalentTo(expectedResult, opt => opt.Excluding(e => e.VerifiedDateTime));
            result.Any(a => a.VerifiedDateTime == null).Should().BeFalse();



        }


        [Fact]
        public async Task Expense_Images_With_All_Mismatching_Images_Shouldnt_Verify_Any()
        {
            //Arrange
            _catImage1 = await
                EmbeddedData.GetByteArrayAsync($"ExpensifyImporter.UnitTests.Modules.IO.Data.{Constants.CatImageFile1}",
                    Assembly.GetAssembly(typeof(ExpenseImageVerifierTests)));

            _catImage2 = await
               EmbeddedData.GetByteArrayAsync($"ExpensifyImporter.UnitTests.Modules.IO.Data.{Constants.CatImageFile2}",
                   Assembly.GetAssembly(typeof(ExpenseImageVerifierTests)));

            _catImage3 = await
               EmbeddedData.GetByteArrayAsync($"ExpensifyImporter.UnitTests.Modules.IO.Data.{Constants.CatImageFile3}",
                   Assembly.GetAssembly(typeof(ExpenseImageVerifierTests)));


            var queryResult = new List<ExpenseImageBatchQueryResult>
            {
                new(_item1Id,Constants.CatImageUrl1,_catImage1),
                new(_item2Id,Constants.CatImageUrl2,_catImage2),
                new(_item3Id,Constants.CatImageUrl3,_catImage3)
            };

            var downloadResult = new List<ExpensifyImageDownloadResult>
            {
                new(_item1Id,_catImage2),
                new(_item2Id,_catImage3),
                new(_item3Id,_catImage1)
            };

            var expectedResult = new List<ExpenseImageVerificationResult>
            {
                new(_item1Id,false,null),
                new(_item2Id,false,null),
                new(_item3Id,false,null),
            };

            //Act 
            var result = _sut.Execute(queryResult, downloadResult);


            //Assert
            result.Should().BeEquivalentTo(expectedResult);
            result.Any(a => a.VerifiedDateTime == null).Should().BeTrue();



        }

        [Fact]
        public async Task Expense_Images_With_Some_Mismatching_Images_Should_Verify_Valid_Ones()
        {
            //Arrange
            _catImage1 = await
                EmbeddedData.GetByteArrayAsync($"ExpensifyImporter.UnitTests.Modules.IO.Data.{Constants.CatImageFile1}",
                    Assembly.GetAssembly(typeof(ExpenseImageVerifierTests)));

            _catImage2 = await
               EmbeddedData.GetByteArrayAsync($"ExpensifyImporter.UnitTests.Modules.IO.Data.{Constants.CatImageFile2}",
                   Assembly.GetAssembly(typeof(ExpenseImageVerifierTests)));

            _catImage3 = await
             EmbeddedData.GetByteArrayAsync($"ExpensifyImporter.UnitTests.Modules.IO.Data.{Constants.CatImageFile3}",
                 Assembly.GetAssembly(typeof(ExpenseImageVerifierTests)));

            var queryResult = new List<ExpenseImageBatchQueryResult>
            {
                new(_item1Id,Constants.CatImageUrl1,_catImage1),
                new(_item2Id,Constants.CatImageUrl2,_catImage2),
                new(_item3Id,Constants.CatImageUrl3,_catImage3)
            };

            var downloadResult = new List<ExpensifyImageDownloadResult>
            {
                new(_item1Id,_catImage1),
                new(_item2Id,_catImage2),
                new(_item3Id,_catImage2)
            };

            var expectedResult = new List<ExpenseImageVerificationResult>
            {
                new(_item1Id,true,DateTime.UtcNow),
                new(_item2Id,true,DateTime.UtcNow),
                new(_item3Id,false,null),
            };

            //Act 
            var result = _sut.Execute(queryResult, downloadResult);


            //Assert
            result.Should().BeEquivalentTo(expectedResult,opt => opt.Excluding(e=>e.VerifiedDateTime));
            result.Count(c => c.VerifiedDateTime != null).Should().Be(2);
            result.Last().VerifiedDateTime.Should().BeNull();
            



        }
    }
}


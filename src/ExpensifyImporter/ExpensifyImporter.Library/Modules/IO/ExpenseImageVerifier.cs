using System;
using ExpensifyImporter.Library.Modules.Database.Domain;
using ExpensifyImporter.Library.Modules.Expensify;

namespace ExpensifyImporter.Library.Modules.IO
{
	public record ExpenseImageVerificationResult(Guid Id,bool Result, DateTime? VerifiedDateTime):EntityRecord(Id);
	public class ExpenseImageVerifier
	{
		public  IEnumerable<ExpenseImageVerificationResult> Execute(
            IEnumerable<ExpenseImageBatchQueryResult> queryResult,
            IEnumerable<ExpensifyImageDownloadResult> downloadResult)
		{
			var result = queryResult.Select(s => {

				//get the matching downloadResult
				var downloadResultImage = downloadResult.Single(single => single.Id == s.Id);
				//verify they match
				var verificationResult = Enumerable.SequenceEqual(
                    first: s.ReceiptImage,
                    second: downloadResultImage.FileContents);

				//return the result
				return new ExpenseImageVerificationResult(s.Id,verificationResult, verificationResult? DateTime.UtcNow:null);
			});

			return result;
		}

	}
}


using System;
using ExpensifyImporter.Library.Modules.Expensify;

namespace ExpensifyImporter.Library.Modules.IO
{
	public record ExpenseImageVerificationResult(Guid id,bool result, DateTime? verifiedDateTime);
	public class ExpenseImageVerifier
	{
		public  IEnumerable<ExpenseImageVerificationResult> Execute(
            IEnumerable<ExpenseImageBatchQueryResult> queryResult,
            IEnumerable<ExpensifyImageDownloadResult> downloadResult)
		{
			var result = queryResult.Select(s => {

				//get the matching downloadResult
				var downloadResultImage = downloadResult.Single(single => single.ExpenseId == s.ExpenseId);
				//verify they match
				var verificationResult = Enumerable.SequenceEqual(
                    first: s.ReceiptImage,
                    second: downloadResultImage.FileContents);

				//return the result
				return new ExpenseImageVerificationResult(s.ExpenseId,verificationResult, verificationResult? DateTime.UtcNow:null);
			});

			return result;
		}

	}
}


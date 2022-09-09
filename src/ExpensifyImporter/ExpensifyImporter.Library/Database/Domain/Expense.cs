using System.ComponentModel.DataAnnotations;

namespace ExpensifyImporter.Library.Database.Domain
{
    
    public class Expense
    {
        [Key]
        public Guid Id { get; set; }

        public int ExpenseId { get; set; }

        public int ReceiptId { get; set; }


        public DateTime Date { get; set; }

        public string? Merchant { get; set; }
        public decimal Amount { get; set; }

        public string? Category { get; set; }

        public string? Description { get; set; }

        public string? ReImbursable { get; set; }

        public string? Currency { get; set; }

        public string? Receipt { get; set; }

        public string? ReceiptDirect { get; set; }

    }
}
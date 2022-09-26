using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace ExpensifyImporter.Database.Domain
{
    public class Expense
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }


        [Column("transaction_datetime")]
        public DateTime TransactionDateTime { get; set; }

        [Column("merchant")]
        public string? Merchant { get; set; }

        [Column("amount")]
        public decimal Amount { get; set; }

        [Column("category")]
        public string? Category { get; set; }

        [Column("description")]
        public string? Description { get; set; }

        [Column("receipt_url")]
        public string? ReceiptUrl { get; set; }

        [Column("receipt_image")]
        public byte[]? ReceiptImage { get; set; }
        
        [Column("company_id")]
        public int CompanyId { get; set; }

    }
}
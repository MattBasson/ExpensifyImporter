
using ExpensifyImporter.Database.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpensifyImporter.Database
{
    public class ExpensifyContext : DbContext
    {
        public DbSet<Expense> Expense { get; set; }

        public ExpensifyContext(DbContextOptions<ExpensifyContext> options) : base(options)
        {

        }
       

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasCharSet("utf8");
            modelBuilder.UseCollation("utf8_general_ci");

            modelBuilder.Entity<Expense>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Merchant).HasColumnType("nvarchar(1000)");
                entity.Property(e => e.Description).HasColumnType("nvarchar(2000)");
                entity.Property(e => e.Category).HasColumnType("nvarchar(1000)");
                entity.Property(e => e.ReceiptImage).HasColumnType("MEDIUMBLOB");

            });
        }
    }
}

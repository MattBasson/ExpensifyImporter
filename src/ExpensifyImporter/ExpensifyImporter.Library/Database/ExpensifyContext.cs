using ExpensifyImporter.Library.Database.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpensifyImporter.Library.Database
{
    public class ExpensifyContext : DbContext
    {
        public DbSet<Expense> Expense { get; set; }

        public ExpensifyContext(DbContextOptions<ExpensifyContext> options) : base(options)
        {

        }
    }
}

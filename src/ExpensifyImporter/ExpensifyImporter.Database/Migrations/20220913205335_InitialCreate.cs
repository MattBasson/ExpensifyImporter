using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpensifyImporter.Database.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8");

            migrationBuilder.CreateTable(
                name: "Expense",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    expense_id = table.Column<int>(type: "int", nullable: false),
                    receipt_id = table.Column<int>(type: "int", nullable: false),
                    transaction_datetime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    merchant = table.Column<string>(type: "nvarchar(1000)", nullable: true),
                    amount = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    category = table.Column<string>(type: "nvarchar(1000)", nullable: true),
                    description = table.Column<string>(type: "nvarchar(2000)", nullable: true),
                    receipt_url = table.Column<string>(type: "longtext", nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expense", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("Relational:Collation", "utf8_general_ci");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Expense");
        }
    }
}

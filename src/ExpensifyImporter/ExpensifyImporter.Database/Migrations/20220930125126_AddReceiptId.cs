using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpensifyImporter.Database.Migrations
{
    public partial class AddReceiptId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "receipt_id",
                table: "Expense",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Expense_receipt_id",
                table: "Expense",
                column: "receipt_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_Expense_receipt_id",
                table: "Expense");

            migrationBuilder.DropColumn(
                name: "receipt_id",
                table: "Expense");
        }
    }
}

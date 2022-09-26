using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpensifyImporter.Database.Migrations
{
    public partial class RemoveExpenseID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "expense_id",
                table: "Expense");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "expense_id",
                table: "Expense",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}

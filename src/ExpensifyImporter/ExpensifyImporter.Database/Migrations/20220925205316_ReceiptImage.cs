using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpensifyImporter.Database.Migrations
{
    public partial class ReceiptImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "receipt_image",
                table: "Expense",
                type: "MEDIUMBLOB",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "receipt_image",
                table: "Expense");
        }
    }
}

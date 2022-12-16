using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpensifyImporter.Database.Migrations
{
    public partial class image_verified : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "image_verified",
                table: "Expense",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "image_verified_datetime",
                table: "Expense",
                type: "DATETIME",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "image_verified",
                table: "Expense");

            migrationBuilder.DropColumn(
                name: "image_verified_datetime",
                table: "Expense");
        }
    }
}

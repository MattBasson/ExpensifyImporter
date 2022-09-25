﻿// <auto-generated />
using System;
using ExpensifyImporter.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ExpensifyImporter.Database.Migrations
{
    [DbContext(typeof(ExpensifyContext))]
    [Migration("20220925212904_CompanyId")]
    partial class CompanyId
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseCollation("utf8_general_ci")
                .HasAnnotation("ProductVersion", "6.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.HasCharSet(modelBuilder, "utf8");

            modelBuilder.Entity("ExpensifyImporter.Database.Domain.Expense", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)")
                        .HasColumnName("id");

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(65,30)")
                        .HasColumnName("amount");

                    b.Property<string>("Category")
                        .HasColumnType("nvarchar(1000)")
                        .HasColumnName("category");

                    b.Property<int>("CompanyId")
                        .HasColumnType("int")
                        .HasColumnName("company_id");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(2000)")
                        .HasColumnName("description");

                    b.Property<int>("ExpenseId")
                        .HasColumnType("int")
                        .HasColumnName("expense_id");

                    b.Property<string>("Merchant")
                        .HasColumnType("nvarchar(1000)")
                        .HasColumnName("merchant");

                    b.Property<byte[]>("ReceiptImage")
                        .HasColumnType("MEDIUMBLOB")
                        .HasColumnName("receipt_image");

                    b.Property<string>("ReceiptUrl")
                        .HasColumnType("longtext")
                        .HasColumnName("receipt_url");

                    b.Property<DateTime>("TransactionDateTime")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("transaction_datetime");

                    b.HasKey("Id");

                    b.ToTable("Expense");
                });
#pragma warning restore 612, 618
        }
    }
}

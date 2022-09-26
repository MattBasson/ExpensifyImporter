using ExpensifyImporter.Database;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace ExpensifyImporter.UnitTests;

public static class SQLiteHelper
{
    private const string InMemoryConnectionString = "Filename=:memory:;Foreign Keys=False";

    public static ExpensifyContext CreateSqliteContext(string connectionString = InMemoryConnectionString)
    {
        var connection = new SqliteConnection(connectionString);
        connection.Open();

        var options = new DbContextOptionsBuilder<ExpensifyContext>();
        options.UseSqlite(connection);

        var dbContext = new ExpensifyContext(options.Options);
        dbContext.Database.EnsureCreated();
        return dbContext;
    }
}
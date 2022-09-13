using ExpensifyImporter.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpensifyImporter.Database
{
    public class ExpensifyContextFactory : IDesignTimeDbContextFactory<ExpensifyContext>
    {
        public ExpensifyContext CreateDbContext(string[] args)
        {

            var configurationBuilder = new ConfigurationBuilder();
            var configuration = configurationBuilder             
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .AddUserSecrets<ExpensifyContext>()
                .AddEnvironmentVariables()
                .Build();

            
            var optionsBuilder = new DbContextOptionsBuilder<ExpensifyContext>();          


            var connectionString = new MySqlConnectionStringBuilder()
            {
                Server = configuration["MySql:Server"],
                Port = configuration.GetValue<uint>("MySql:Port"),
                Database = configuration["MySql:Database"],
                //Set with user secrets (for now)
                UserID = configuration["MySql:User"],
                Password = configuration["MySql:Password"],
                ConnectionTimeout = 500

            }.ToString();
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), builder =>
            {
                builder.CommandTimeout(60);                
            });

            return new ExpensifyContext(optionsBuilder.Options);
        }
    }
}

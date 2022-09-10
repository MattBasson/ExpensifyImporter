// See https://aka.ms/new-console-template for more information
using ExpensifyImporter.Application;
using ExpensifyImporter.Application.Data;
using ExpensifyImporter.Library.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MySqlConnector;
using System.Reflection;
using Serilog;
using Microsoft.Extensions.Logging;

var flagExtractor = new FlagExtractor(args);

var flags = flagExtractor.GetSupportedFlags();

if (!flags.Any())
{
    Console.WriteLine("Please specify an argument or flag or use help -h or -help");
    Console.Read();
}

if(flags.Any(a=>a.Flag == Flags.Help))
{
    Console.WriteLine(DataHelper.Get("ExpensifyImporter.Application.Data.Content.HelpContent.txt"));
    Console.Read();
}

if(flags.Any(a=>a.Flag == Flags.Directory))
{
    //Directory flag specified processing can begin and resource newing up can start.

    var builder = Host.CreateDefaultBuilder(args)
        .ConfigureAppConfiguration((hostingContext, config) =>
        {
            config.SetBasePath(Directory.GetCurrentDirectory());
            config.AddJsonFile("appsettings.json", true, true);
            config.AddEnvironmentVariables();
            config.AddUserSecrets(Assembly.GetExecutingAssembly(), true);
        })
        .ConfigureLogging(logging => logging.ClearProviders())
        .UseSerilog((hostingContext, config) => { config.ReadFrom.Configuration(hostingContext.Configuration); })
        .ConfigureServices((hostContext,services) =>
        {
            IConfiguration configuration = hostContext.Configuration;

            //Configure services here...


            services.AddDbContext<ExpensifyContext>((provider, options) =>
            {   
                var connectionString = new MySqlConnectionStringBuilder()
                {
                    Server = configuration["MySql:Server"],
                    Port = configuration.GetValue<uint>("MySql:Port"),
                    Database = configuration["MySql:Database"],
                    ConnectionTimeout = 500

                }.ToString();
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), builder =>
                {
                    builder.CommandTimeout(60);
                });
            });
        });

    using IHost host = builder.Build();

    

    await host.RunAsync();

}   



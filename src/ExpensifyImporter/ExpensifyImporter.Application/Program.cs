// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MySqlConnector;
using System.Reflection;
using Serilog;
using Microsoft.Extensions.Logging;
using ExpensifyImporter.Database;
using ExpensifyImporter.Library.Modules.Flags;
using ExpensifyImporter.Library.Modules.Flags.Domain;
using ExpensifyImporter.Library.Modules.IO;
using ExpensifyImporter.Application;
using ExpensifyImporter.Library.Modules.Excel;
using ExpensifyImporter.Library.Modules.Expensify;
using ExpensifyImporter.Library.Modules.Sequencing;

var flagFactory = new FlagFactory(args);

var flags = flagFactory.GetSupportedFlags();

if (!flags.Any())
{
    Console.WriteLine("Please specify an argument or flag or use help -h or -help");
    Console.Read();
}

if(flags.Any(a=>a.Flag == FlagType.Help))
{
    Console.WriteLine(EmbeddedData.Get("ExpensifyImporter.Library.Content.Flags.HelpContent.txt"));
    Console.Read();
}

if(flags.Any(a=>a.Flag == FlagType.Directory))
{
    //Directory flag specified processing can begin and resource newing up can start.
    var fileWatchPath = flags.First(f => f.Flag == FlagType.Directory).FlagValue;

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
                    //Set with user secrets (for now)
                    UserID = configuration["MySql:User"],
                    Password = configuration["MySql:Password"],
                    ConnectionTimeout = 3000

                }.ToString();
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), builder =>
                {
                    builder.CommandTimeout(60);
                });
            });
            services.AddScoped<ExcelReader>();
            services.AddScoped<ExcelDtoMapper>();
            services.AddScoped<ExpensifyModelExcelDtoMapper>();
            services.AddTransient(x => new ExcelFileWatcher(
                x.GetRequiredService<ILogger<ExcelFileWatcher>>(),
                fileWatchPath));
            services.AddScoped<ExcelToDatabaseSequencer>();
            services.AddHostedService<Worker>();
        });

    using IHost host = builder.Build();

    

    await host.RunAsync();

}   



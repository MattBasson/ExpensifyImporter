// See https://aka.ms/new-console-template for more information

using System.Reflection;
using System.Web;
using ExpensifyImporter.Application;
using ExpensifyImporter.Database;
using ExpensifyImporter.Library.Domain;
using ExpensifyImporter.Library.Modules.Database;
using ExpensifyImporter.Library.Modules.Excel;
using ExpensifyImporter.Library.Modules.Expensify;
using ExpensifyImporter.Library.Modules.IO;
using ExpensifyImporter.Library.Modules.Sequencing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MySqlConnector;
using Serilog;

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
    .ConfigureServices((hostContext, services) =>
    {
        var configuration = hostContext.Configuration;
        //Directory flag specified processing can begin and resource newing up can start.
        var fileWatchPath = configuration.GetValue<string>("Worker:DataDirectory");
        //Configure services here...

        services
        .Configure<WorkerConfiguration>(model =>
        {
            var config = configuration.GetSection("Worker");
            model.Interval = config.GetValue<int>("Interval");
            model.DataDirectory = config.GetValue<string>("DataDirectory");
            //set via user secret.
            model.ExpensifyAuthToken = config.GetValue<string>("ExpensifyAuthToken");
            model.ImageDownloadBatchSize = config.GetValue<int>("ImageDownloadBatchSize");
        })
        .Configure<FeatureFlagsConfiguration>(model =>
        {
            var config = configuration.GetSection("FeatureFlags");
            model.WatchDirectory = config.GetValue<bool>("WatchDirectory");
            model.PollDirectory = config.GetValue<bool>("PollDirectory");
            model.DownloadImages = config.GetValue<bool>("DownloadImages");
            model.VerifyImages = config.GetValue<bool>("VerifyImages");

        })
        .AddDbContext<ExpensifyContext>((provider, options) =>
        {
            var connectionString = new MySqlConnectionStringBuilder
            {
                Server = configuration["MySql:Server"],
                Port = configuration.GetValue<uint>("MySql:Port"),
                Database = configuration["MySql:Database"],
                //Set with user secrets
                UserID = configuration["MySql:User"],
                Password = configuration["MySql:Password"],
                ConnectionTimeout = 3000
            }.ToString();
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString),
                builder => { builder.CommandTimeout(60); });
        })
        .AddScoped<ExcelReader>()
        .AddScoped<ExcelDtoMapper>()
        .AddScoped<ExpensifyModelExcelDtoMapper>()
        .AddTransient(x => new ExcelFileWatcher(
            x.GetRequiredService<ILogger<ExcelFileWatcher>>(),
            fileWatchPath))
        .AddScoped<ExpenseDuplicates>()
        .AddScoped<ExcelToDatabaseSequencer>()
        .AddScoped<ImageDownloader>()
        .AddScoped<ExpenseImageBatchQuery>()
        .AddScoped<ExpenseImageBatchCommand>()
        .AddScoped<ExpensifyImageDownloader>()
        .AddScoped<ImageToDatabaseSequencer>()
        .AddHostedService<Worker>();

        services.AddHttpClient<ImageDownloader>(client =>
        {
            
            
            client.DefaultRequestHeaders.Add("Cookie", $"authToken={configuration["Worker:ExpensifyAuthToken"]}");
        });
    });

using var host = builder.Build();

await host.RunAsync();
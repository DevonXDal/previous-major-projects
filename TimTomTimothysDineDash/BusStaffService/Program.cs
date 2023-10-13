using BarebonesEFCoreRepositoryPatternStarter.Data;
using BarebonesRabbitMQImplementation;
using BarebonesRabbitMQImplementation.Helpers.RabbitMQ;
using BarebonesRabbitMQImplementationLibrary.Models;
using BarebonesRabbitMQImplementationLibrary.Models.DTO;
using BusStaffService.Database.Data;
using BusStaffService.Database.Models;
using BusStaffService.EventHandlers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;
using System;
using System.Reflection;

namespace BusStaffService
{
    internal class Program : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        private const string MICROSERVICE_NAME = "Bus Staff Service"; // Identifies this microservice type for logging
        private static readonly string RUNTIME_UUID = Guid.NewGuid().ToString(); // Identifies this instance of the microservice while it runs

        static async Task Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection, args);

            var serviceProvider = serviceCollection.BuildServiceProvider();

            Log.Information($"Services configured successfully during startup");

            try
            {

                ListenForEvents(serviceProvider);
                await Task.Delay(Timeout.Infinite);
            } catch (Exception ex)
            {
                Log.Fatal(ex, "This service has crashed");
            } finally
            {
                Log.Warning("Shutting down...");
                Log.CloseAndFlush();
            }
        }

        private static void ConfigureServices(IServiceCollection services, string[] cliArgs)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true)
                .AddEnvironmentVariables()
                .AddCommandLine(cliArgs)
                .Build();

            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithExceptionDetails()
                .WriteTo.Debug()
                .WriteTo.Console()
                .WriteTo.Elasticsearch(ConfigureElasticSink(configuration, environment))
                .Enrich.WithProperty("Environment", environment)
                .Enrich.WithProperty("Microservice", MICROSERVICE_NAME)
                .Enrich.WithProperty("Runtime UUID", RUNTIME_UUID)
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            services.AddSingleton<IConfiguration>(configuration)
                .AddLogging(configure => configure.AddSerilog())
                .AddSingleton<RabbitMQEnv>()
                .AddSingleton<Env>()
                .AddSingleton<IMessageProducer, RabbitMQProducer>()
                .AddSingleton<IMessageSubscriber<string>, RabbitMQSubscriber>();

            ServiceProvider tempServiceProvider = services.BuildServiceProvider();

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseMySql(
                tempServiceProvider.GetService<Env>().MainDb,
                new MariaDbServerVersion(new Version(10, 6, 7)),
                mySqlOptions =>
                {
                    mySqlOptions.EnableRetryOnFailure(
                        10,
                        TimeSpan.FromSeconds(2),
                        new List<int> { }
                    );
                }
                );
            });

            // Services needing the DbContext added
            services.AddScoped<RepositoryBase<ApplicationDbContext, Table>>();

            services.AddScoped<StatusUpdateEventHandler>();
            services.AddScoped<NoticeTableHasPaidForOrderAndLeftEventHandler>();
            services.AddScoped<TableHasBeenCleanedAndAwaitsAnotherGroup>();

        }

        /// <summary>
        /// Begins the process of listening to future events related to this microservice.
        /// </summary>
        /// <param name="services">The service injector, used to get a handler to react to an event</param>
        private static void ListenForEvents(ServiceProvider services)
        {
            var queueSubscriber = services.GetRequiredService<IMessageSubscriber<string>>();

            // Retrieve the current state of the database data
            queueSubscriber.Subscribe<DataWithCorrelation>(StatusUpdateEventHandler.EVENT,
                onNext: (message) =>
                {
                    var eventHandler = services.GetRequiredService<StatusUpdateEventHandler>();

                    eventHandler.Handle(message);
                },
                onError: (err) =>
                {
                    LogGenericSubscribedEventError(StatusUpdateEventHandler.EVENT, err);
                }
            );

            // Mark as table as clean and ready for use
            queueSubscriber.Subscribe<SingleStringDTO>(TableHasBeenCleanedAndAwaitsAnotherGroup.EVENT,
                onNext: (message) =>
                {
                    var eventHandler = services.GetRequiredService<TableHasBeenCleanedAndAwaitsAnotherGroup>();

                    eventHandler.Handle(message);
                },
                onError: (err) =>
                {
                    LogGenericSubscribedEventError(TableHasBeenCleanedAndAwaitsAnotherGroup.EVENT, err);
                }
            );

            // Watch for tables being paid for and left empty
            queueSubscriber.Subscribe<SingleStringDTO>(NoticeTableHasPaidForOrderAndLeftEventHandler.EVENT,
                onNext: (message) =>
                {
                    var eventHandler = services.GetRequiredService<NoticeTableHasPaidForOrderAndLeftEventHandler>();

                    eventHandler.Handle(message);
                },
                onError: (err) =>
                {
                    LogGenericSubscribedEventError(NoticeTableHasPaidForOrderAndLeftEventHandler.EVENT, err);
                }
            );
        }

        private static void LogGenericSubscribedEventError(string eventName, Exception err)
        {
            Log.Error($"Could not carry out event: {eventName}", err);
        }

        /// <summary>
        /// https://dev.to/moe23/net-6-webapi-intro-to-elasticsearch-kibana-step-by-step-p9l
        /// 
        /// This configures the elastic sink options in order to include environment information, timestampts, and assembly information.
        /// This helps when filtering logs in elastic search.
        /// </summary>
        /// <param name="configuration">The configuration from the environment variables</param>
        /// <param name="environment">The system's environment including infomation like whether this is being run in production</param>
        /// <returns>The options to configure the elastic searchs sink for Serilog with</returns>
        private static ElasticsearchSinkOptions ConfigureElasticSink(IConfigurationRoot configuration, string environment)
        {
            return new ElasticsearchSinkOptions(new Uri(configuration["ElasticConfiguration:Uri"])) // This happens before the Env can be provided
            {
                AutoRegisterTemplate = true,
                AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv8,
                IndexFormat = $"{Assembly.GetExecutingAssembly().GetName().Name.ToLower().Replace(".", "-")}-{environment?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}"
            };
        }

        public ApplicationDbContext CreateDbContext(string[] args)
        {
            // https://stackoverflow.com/questions/48363173/how-to-allow-migration-for-a-console-application
            var configurationBuilder = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("appsettings.json", optional: true)
              .AddCommandLine(args)
              .AddEnvironmentVariables();

            IConfigurationRoot configuration = configurationBuilder.Build();
            string connectionString = (new Env(configuration)).MainDb;

            DbContextOptionsBuilder<ApplicationDbContext> optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseMySql(
                connectionString,
                new MariaDbServerVersion(new Version(10, 6, 7)),
                mySqlOptions =>
                {
                    mySqlOptions.EnableRetryOnFailure(
                        10,
                        TimeSpan.FromSeconds(2),
                        new List<int> { }
                    );
                    mySqlOptions.MigrationsAssembly("BusStaffService");
                }
                );

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
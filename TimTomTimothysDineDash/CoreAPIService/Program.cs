using BarebonesEFCoreRepositoryPatternStarter.Data;
using BarebonesRabbitMQImplementation;
using BarebonesRabbitMQImplementation.Helpers.RabbitMQ;
using CoreAPIService.Database.Data;
using CoreAPIService.Database.Models;
using CoreAPIService.Helpers;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;
using System;
using System.Reflection;

namespace CoreAPIService
{
    public class Program
    {
        private const string MICROSERVICE_NAME = "Core API Service"; // Identifies this microservice type for logging
        private static readonly string RUNTIME_UUID = Guid.NewGuid().ToString(); // Identifies this instance of the microservice while it runs

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            builder.Configuration.AddJsonFile("appsettings.json", true).AddEnvironmentVariables().AddCommandLine(args);

            // Add services to the container.
            builder.Services.AddCors();
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Add message producer and subscriber
            builder.Services.AddSingleton<Env>();
            builder.Services.AddSingleton<RabbitMQEnv>();
            builder.Services.AddSingleton<IMessageProducer, RabbitMQProducer>();
            builder.Services.AddSingleton<IMessageSubscriber<string>, RabbitMQSubscriber>();

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseMySql(
                builder.Services.BuildServiceProvider().GetService<Env>().MainDb,
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
            builder.Services.AddScoped<RepositoryBase<ApplicationDbContext, MessageFeed>>();
            builder.Services.AddScoped<MessageFeedHandler>();

            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithExceptionDetails()
                .WriteTo.Debug()
                .WriteTo.Console()
                .WriteTo.Elasticsearch(ConfigureElasticSink(builder.Configuration, environment))
                .Enrich.WithProperty("Environment", environment)
                .Enrich.WithProperty("Microservice", MICROSERVICE_NAME)
                .Enrich.WithProperty("Runtime UUID", RUNTIME_UUID)
                .ReadFrom.Configuration(builder.Configuration)
                .CreateLogger();

            builder.Services.AddLogging(configure => configure.AddSerilog());

            builder.Services.AddApiVersioning(o =>
            {
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
                o.ReportApiVersions = true;
                o.ApiVersionReader = ApiVersionReader.Combine(
                    new QueryStringApiVersionReader("api-version"),
                    new HeaderApiVersionReader("X-Version"),
                    new MediaTypeApiVersionReader("ver"));

            });
            

            var app = builder.Build();

            app.UsePathBase("/api");

            app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true) // allow any origin
                .AllowCredentials());

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
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
    }
}
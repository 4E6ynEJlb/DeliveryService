using Microsoft.EntityFrameworkCore;
using Infrastructure;
using Infrastructure.Interfaces;
using Minio.AspNetCore;
using Serilog;
using Serilog.Sinks.Grafana.Loki;
using Serilog.Events;
using Serilog.Sinks.GrafanaLoki;

namespace API.DIConfiguration
{
    internal static partial class ConfigurationExtensions
    {
        internal static void ConfigureDatabases(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<SQLContext>(options =>
            {
                string? connectionString = builder.Configuration.GetSection("SQLConnectionStrings").GetValue<string>("DefaultConnection");
                string? password = Environment.GetEnvironmentVariable("MSSQL_SA_PASSWORD");
                connectionString = string.Format(connectionString??throw new ArgumentNullException("SQL ConnectionString"), password);
                options.UseSqlServer(connectionString);
            });
            builder.Services.AddScoped<IMongoContext, MongoContext>();
            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = builder.Configuration.GetSection("RedisConnectionOptions").GetValue<string>("Configuration");
                options.InstanceName = builder.Configuration.GetSection("RedisConnectionOptions").GetValue<string>("InstanceName");
            });
            builder.Services.AddMinio(options => 
            { 
                options.AccessKey = builder.Configuration.GetSection("MinIoOptions").GetValue<string>("AccessKey") ?? throw new ArgumentNullException("MinIo AccessKey"); 
                options.SecretKey = builder.Configuration.GetSection("MinIoOptions").GetValue<string>("SecretKey") ?? throw new ArgumentNullException("MinIo SecretKey");
                options.Endpoint = builder.Configuration.GetSection("MinIoOptions").GetValue<string>("Endpoint") ?? throw new ArgumentNullException("MinIo Endpoint");
            });
            var config = builder.Configuration;
            var credentials = new GrafanaLokiCredentials()
            {
                User = builder.Configuration.GetSection("LokiOptions").GetValue<string>("User") ?? throw new ArgumentNullException("Loki User"),
                Password = builder.Configuration.GetSection("LokiOptions").GetValue<string>("Password") ?? throw new ArgumentNullException("Loki Password")
            };
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(config)
                .WriteTo.GrafanaLoki(
                    builder.Configuration.GetSection("LokiOptions").GetValue<string>("URI") ?? throw new ArgumentNullException("Loki URI"),
                    credentials,
                    new Dictionary<string, string> { { "app", "Serilog.Sinks.GrafanaLoki.Sample" } },
                    LogEventLevel.Information
                ).CreateLogger();
            builder.Host.UseSerilog();
            builder.Logging.AddSerilog();
        }
    }
}

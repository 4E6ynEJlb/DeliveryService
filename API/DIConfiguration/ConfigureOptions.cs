using Domain.Models.ApplicationModels;

namespace API.DIConfiguration
{
    internal static partial class ConfigurationExtensions
    {
        internal static void ConfigureOptions(this WebApplicationBuilder builder)
        {
            builder.Configuration.AddJsonFile("DBOptions.json");
            builder.Configuration.AddJsonFile("ConstsOptions.json");
            builder.Services.Configure<MongoDBOptions>(builder.Configuration.GetSection(MongoDBOptions.OptionsName));
            builder.Services.Configure<RepositoryOptions>(builder.Configuration.GetSection(RepositoryOptions.OptionsName));
            builder.Services.Configure<HostAuthOptions>(builder.Configuration.GetSection(HostAuthOptions.OptionsName));
            builder.Services.Configure<TelegramAPIOptions>(builder.Configuration.GetSection(TelegramAPIOptions.OptionsName));
            builder.Services.Configure<ServicesOptions>(builder.Configuration.GetSection(ServicesOptions.OptionsName));
            builder.Services.Configure<MinIoOptions>(builder.Configuration.GetSection(MinIoOptions.OptionsName));
        }
    }
}

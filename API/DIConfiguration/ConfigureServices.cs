using Application.Interfaces;
using Application.Services;
using HostedServices;
using Infrastructure;
using Infrastructure.Interfaces;

namespace API.DIConfiguration
{
    internal static partial class ConfigurationExtensions
    {
        internal static void ConfigureServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IFileClient, FileClient>();
            builder.Services.AddScoped<IUserCacheService, UserCacheService>();
            builder.Services.AddScoped<IProductCacheService, ProductCacheService>();
            builder.Services.AddScoped<IAuditHostService, AuditHostService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<ICardsUserService, CardsUserService>();
            builder.Services.AddScoped<IGoodsAdminService, GoodsAdminService>();
            builder.Services.AddScoped<IGoodsUserService, GoodsUserService>();
            builder.Services.AddScoped<IOrderRestaurantService, OrderRestaurantService>();
            builder.Services.AddScoped<IOrderUserService, OrderUserService>();
            builder.Services.AddScoped<IReportAdminService, ReportAdminService>();
            builder.Services.AddScoped<IReportUserService, ReportUserService>();
            builder.Services.AddScoped<IRestaurantAdminService, RestaurantAdminService>();
            builder.Services.AddScoped<IRestaurantUserService, RestaurantUserService>();
            builder.Services.AddScoped<IUserHostService, UserHostService>();
            builder.Services.AddScoped<IUserUserService, UserUserService>();
            builder.Services.AddSingleton<ITelegramBotApi, TelegramBotAPI>();
            builder.Services.AddHostedService<BotService>();
        }
    }
}

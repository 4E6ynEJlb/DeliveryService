using Domain.Stores;
using Persistence.Repository;

namespace API.DIConfiguration
{
    internal static partial class ConfigurationExtensions
    {
        internal static void ConfigureRepositories(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<ISoldProductStore, SoldProductRepository>();
            builder.Services.AddScoped<IAuditLogStore, AuditLogRepository>();
            builder.Services.AddScoped<IOrderStore, OrderRepository>();
            builder.Services.AddScoped<IReportStore, ReportRepository>();
            builder.Services.AddScoped<ICardStore, CardRepository>();
            builder.Services.AddScoped<IProductStore, ProductRepository>();
            builder.Services.AddScoped<IRestaurantStore, RestaurantRepository>();
            builder.Services.AddScoped<IUserStore, UserRepository>();
        }
    }
}

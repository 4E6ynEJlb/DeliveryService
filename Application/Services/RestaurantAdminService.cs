using Application.Interfaces;
using Domain.Models.Entities.MongoDBEntities;
using Domain.Models.Entities.SQLEntities;
using Domain.Models.VievModels;
using Domain.Stores;

namespace Application.Services
{
    public class RestaurantAdminService : IRestaurantAdminService
    {
        private readonly IRestaurantStore _restaurantStore;
        private readonly IAuditLogStore _auditLogStore;
        public RestaurantAdminService(IRestaurantStore restaurantStore,IAuditLogStore auditLogStore) 
        {
            _restaurantStore = restaurantStore;
            _auditLogStore = auditLogStore;
        }

        public async Task EditRestaurantAuthAsync(string adress, AuthModel authModel, Guid adminId, CancellationToken cancellationToken)
        {
            await _restaurantStore.EditRestaurantAuthAsync(adress, authModel, cancellationToken);
            await _auditLogStore.AddRecordAsync(new AuditLogRecord(adminId, $"{AuditLogExpressions.RESTAURANT_AUTH_CHANGED}{adress}"), cancellationToken);
        }
        public async Task AddRestaurantAsync(Restaurant restaurant, Guid adminId, CancellationToken cancellationToken)
        {
            await _restaurantStore.AddRestaurantAsync(restaurant, cancellationToken);
            await _auditLogStore.AddRecordAsync(new AuditLogRecord(adminId, $"{AuditLogExpressions.RESTAURANT_ADDED}{restaurant.Adress}"), cancellationToken);
        }

        public async Task RemoveRestaurantAsync(string adress, Guid adminId, CancellationToken cancellationToken)
        {
            await _restaurantStore.RemoveRestaurantAsync(adress, cancellationToken);
            await _auditLogStore.AddRecordAsync(new AuditLogRecord(adminId, $"{AuditLogExpressions.RESTAURANT_REMOVED}{adress}"), cancellationToken);
        }
    }
}

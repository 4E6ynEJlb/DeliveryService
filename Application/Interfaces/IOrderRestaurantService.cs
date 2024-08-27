using Domain.Models.VievModels;

namespace Application.Interfaces
{
    public interface IOrderRestaurantService
    {
        public Task<OrderModel> GetOrderByIdAsync(Guid id, CancellationToken cancellationToken);
        public Task<List<OrderModel>> GetOrdersListAsync(int count, Coordinates restaurantCoordinates, CancellationToken cancellationToken);
        public Task AcceptOrderAsync(Guid id, CancellationToken cancellationToken);
        public Task RemoveUnitFromListAsync(Guid id, int article, bool wasCookedEarlier, CancellationToken cancellationToken);
        public Task RemoveOrderAsync(Guid id, CancellationToken cancellationToken);
    }
}

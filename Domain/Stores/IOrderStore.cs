using Domain.Models.Entities.MongoDBEntities;
using Domain.Models.VievModels;

namespace Domain.Stores
{
    public interface IOrderStore
    {
        public Task<Order> GetOrderByIdAsync(Guid id, CancellationToken cancellationToken);
        public Task<List<Order>> GetOrdersByUserIdAsync(Guid id, CancellationToken cancellationToken);
        public Task<List<Order>> GetOrdersListAsync(int count, Coordinates restaurantCoordinates, CancellationToken cancellationToken);
        public Task AddOrderAsync(Order order, CancellationToken cancellationToken);
        public Task RemoveOrderAsync(Guid id, CancellationToken cancellationToken);
        public Task AcceptOrderAsync(Guid id, CancellationToken cancellationToken);
        public Task<DateTime> RemoveUnitFromListAsync(Guid id, int article, CancellationToken cancellationToken);
    }
}

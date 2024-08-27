using Domain.Models.VievModels;

namespace Application.Interfaces
{
    public interface IOrderUserService
    {
        public Task<OrderModel> GetOrderByIdAsync(Guid id, CancellationToken cancellationToken);
        public Task<List<OrderModel>> GetOrdersByUserIdAsync(Guid id, CancellationToken cancellationToken);
        public Task AddOrderAsync(OrderModel order, CancellationToken cancellationToken);
        public Task RemoveOrderAsync(Guid id, CancellationToken cancellationToken);
    }
}

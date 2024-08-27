using Application.Interfaces;
using Domain.Models.Entities.MongoDBEntities;
using Domain.Models.VievModels;
using Domain.Stores;

namespace Application.Services
{
    public class OrderRestaurantService : IOrderRestaurantService
    {
        private readonly IOrderStore _orderStore;
        private readonly ISoldProductStore _soldProductStore;
        private readonly IProductStore _productStore;
        public OrderRestaurantService(IOrderStore orderStore, ISoldProductStore soldProductStore, IProductStore productStore)
        {
            _orderStore = orderStore;
            _soldProductStore = soldProductStore;
            _productStore = productStore;
        }
        public async Task<OrderModel> GetOrderByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return new OrderModel(await _orderStore.GetOrderByIdAsync(id, cancellationToken));
        }

        public async Task<List<OrderModel>> GetOrdersListAsync(int count, Coordinates restaurantCoordinates, CancellationToken cancellationToken)
        {
            List<Order> orders = await _orderStore.GetOrdersListAsync(count, restaurantCoordinates, cancellationToken);
            List<OrderModel> orderModels = new List<OrderModel>();
            foreach (Order order in orders)
            {
                orderModels.Add(new OrderModel(order));
            }
            return orderModels;
        }
        public async Task AcceptOrderAsync(Guid id, CancellationToken cancellationToken)
        {
            await _orderStore.AcceptOrderAsync(id, cancellationToken);
        }
        public async Task RemoveOrderAsync(Guid id, CancellationToken cancellationToken)
        {
            await _orderStore.RemoveOrderAsync(id, cancellationToken);
        }

        public async Task RemoveUnitFromListAsync(Guid id, int article, bool wasCookedEarlier, CancellationToken cancellationToken)
        {
            DateTime timeMarker = await _orderStore.RemoveUnitFromListAsync(id, article, cancellationToken);
            if (!wasCookedEarlier)
            {
                await _soldProductStore.AddSoldProductAsync(new SoldProduct(article), cancellationToken);
                TimeOnly cookingTime = new TimeOnly((DateTime.Now - timeMarker).Ticks);
                await _productStore.UpdateCookingTimeAsync(article, cookingTime, cancellationToken);
            }            
        }
    }
}

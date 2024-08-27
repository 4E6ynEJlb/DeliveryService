using Application.Interfaces;
using Domain.Models.Entities.SQLEntities;
using Domain.Models.VievModels;
using Domain.Stores;
using Domain.Models.Entities.MongoDBEntities;
using Domain.Models.ApplicationModels.Exceptions;

namespace Application.Services
{
    public class OrderUserService : IOrderUserService
    {
        private readonly IOrderStore _orderStore;
        private readonly IUserStore _userStore;
        private readonly ICardStore _cardStore;
        public OrderUserService(IOrderStore orderStore, IUserStore userStore, ICardStore cardStore) 
        {
            _orderStore = orderStore;
            _userStore = userStore;
            _cardStore = cardStore;
        }
        public async Task<OrderModel> GetOrderByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return new OrderModel(await _orderStore.GetOrderByIdAsync(id, cancellationToken));
        }
        public async Task<List<OrderModel>> GetOrdersByUserIdAsync(Guid id, CancellationToken cancellationToken)
        {
            List<OrderModel> orderModels = new List<OrderModel>();
            List<Order> orders = await _orderStore.GetOrdersByUserIdAsync(id, cancellationToken);
            foreach (Order order in orders)
                orderModels.Add(new OrderModel(order));
            return orderModels;
        }
        public async Task AddOrderAsync(OrderModel order, CancellationToken cancellationToken)
        {
            User user = await _userStore.GetUserByIdAsync(order.UserId, cancellationToken);
            if ((await _cardStore.GetUserCardsAsync(user.Id, cancellationToken)).Where(c=>c.Number == order.PaymentCard).Count()==0)
                throw new InvalidCardNumberException();
            await _orderStore.AddOrderAsync(order.ToOrder(), cancellationToken);
        }

        public async Task RemoveOrderAsync(Guid id, CancellationToken cancellationToken)
        {
            await _orderStore.RemoveOrderAsync(id, cancellationToken);
        }
    }
}

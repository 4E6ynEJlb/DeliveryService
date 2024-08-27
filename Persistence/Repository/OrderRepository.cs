using Domain.Models.Entities.MongoDBEntities;
using Domain.Models.Entities.SQLEntities;
using Domain.Models.VievModels;
using Domain.Stores;
using Infrastructure;
using Infrastructure.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Data;

namespace Persistence.Repository
{
    public class OrderRepository : IOrderStore
    {
        private readonly IMongoContext _context;
        public OrderRepository(IMongoContext context) 
        {
            _context = context;
        }

        public async Task<Order> GetOrderByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            Order? order = await _context.Orders.AsQueryable().FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
            if (order == null)
                throw new DoesNotExistException(typeof(Order));
            return order;
        }

        public async Task<List<Order>> GetOrdersByUserIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Orders.AsQueryable().Where(o => o.UserId == id).ToListAsync();
        }

        public async Task<List<Order>> GetOrdersListAsync(int count, Coordinates restaurantCoordinates, CancellationToken cancellationToken)
        {
            return await _context.Orders.AsQueryable().OrderBy(o => o.Coordinates.CalcDistance(restaurantCoordinates)).Take(count).ToListAsync(cancellationToken);
        }

        public async Task AcceptOrderAsync(Guid id, CancellationToken cancellationToken)
        {
            Order? order = await _context.Orders.AsQueryable().Where(o => !o.IsCooking).FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
            if (order == null)
                throw new DoesNotExistException(typeof(Order));
            order.IsCooking = true;
#pragma warning disable CS0618 // Тип или член устарел
            await _context.Orders.ReplaceOneAsync(new BsonDocument("Id", id), order, cancellationToken: cancellationToken);
#pragma warning restore CS0618 // Тип или член устарел
        }

        public async Task AddOrderAsync(Order order, CancellationToken cancellationToken)
        {
#pragma warning disable CS0618 // Тип или член устарел
            await _context.Orders.InsertOneAsync(order, cancellationToken);
#pragma warning restore CS0618 // Тип или член устарел
        }

        public async Task RemoveOrderAsync(Guid id, CancellationToken cancellationToken)
        {
#pragma warning disable CS0618 // Тип или член устарел
            await _context.Orders.DeleteOneAsync(new BsonDocument("Id", id), cancellationToken);
#pragma warning restore CS0618 // Тип или член устарел
        }

        public async Task<DateTime> RemoveUnitFromListAsync(Guid id, int article, CancellationToken cancellationToken)
        {
            Order? order = await _context.Orders.AsQueryable().FirstOrDefaultAsync(o => o.Id == id && o.IsCooking, cancellationToken);
            if (order == null)
                throw new DoesNotExistException(typeof(Order));
            var goodsList = order.GoodsList;
            if (!goodsList.TryGetValue(article, out int count))
                throw new DoesNotExistException(typeof(Product));
            if (count <= 1)
                goodsList.Remove(article);
            else
                goodsList[article]--;
            DateTime oldTimeMarker = order.TimeMarker!.Value;
            order.TimeMarker = DateTime.Now;
#pragma warning disable CS0618 // Тип или член устарел
            await _context.Orders.ReplaceOneAsync(new BsonDocument("Id", id), order, cancellationToken: cancellationToken);
#pragma warning restore CS0618 // Тип или член устарел
            return oldTimeMarker;
        }
    }
}

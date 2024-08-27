using Domain.Models.ApplicationModels;
using Domain.Models.Entities.MongoDBEntities;
using Domain.Stores;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Persistence.Repository
{
    public class SoldProductRepository : ISoldProductStore
    {
        private readonly IMongoContext _context;
        private readonly int _hotGoodsExpirationHours;
        public SoldProductRepository(IMongoContext context, IOptions<RepositoryOptions> options)
        {
            _context = context;
            _hotGoodsExpirationHours = options.Value.HotGoodsExpirationHours;
        }
        public async Task<List<int>> GetHotArticleListAsync(int goodsCount, CancellationToken cancellationToken)
        {
            var query = from SoldProduct in _context.SoldGoods.AsQueryable()
                         group SoldProduct by SoldProduct.Article into g
                         select new { Article = g.Key, Count = g.Count() };
            return await query.OrderByDescending(x => x.Count).Select(x => x.Article).Take(goodsCount).ToListAsync(cancellationToken);
        }

        public async Task AddSoldProductAsync(SoldProduct soldProduct, CancellationToken cancellationToken)
        {
            soldProduct.ExpireAt = DateTime.Now.AddHours(_hotGoodsExpirationHours);
#pragma warning disable CS0618 // Тип или член устарел
            await _context.SoldGoods.InsertOneAsync(soldProduct, cancellationToken);
#pragma warning restore CS0618 // Тип или член устарел
        }
    }
}

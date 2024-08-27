using Domain.Models.Entities.MongoDBEntities;

namespace Domain.Stores
{
    public interface ISoldProductStore
    {
        public Task<List<int>> GetHotArticleListAsync(int goodsCount, CancellationToken cancellationToken);
        public Task AddSoldProductAsync(SoldProduct soldProduct, CancellationToken cancellationToken);
    }
}

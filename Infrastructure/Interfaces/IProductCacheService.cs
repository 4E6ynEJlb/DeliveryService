using Domain.Models.Entities.SQLEntities;

namespace Infrastructure.Interfaces
{
    public interface IProductCacheService
    {
        public Task<Product?> GetAsync(int article, CancellationToken cancellationToken);
        public Task SaveAsync(Product product, CancellationToken cancellationToken);
        public Task RemoveAsync(int article, CancellationToken cancellationToken);
        public Task UpdateAsync(Product product, CancellationToken cancellationToken);
    }
}

using Domain.Models.ApplicationModels;
using Domain.Models.Entities.SQLEntities;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Infrastructure
{
    public class ProductCacheService : IProductCacheService
    {
        private readonly IDistributedCache _cache;
        private readonly DistributedCacheEntryOptions _cacheOptions;
        public ProductCacheService(IDistributedCache cache, IOptions<RepositoryOptions> repositoryOptions)
        {
            _cache = cache;
            _cacheOptions = new DistributedCacheEntryOptions() { SlidingExpiration = TimeSpan.FromMinutes(repositoryOptions.Value.CacheExpirationMins) };
        }
        public async Task<Product?> GetAsync(int article, CancellationToken cancellationToken)
        {
            Product? product = null;
            string? productString = await _cache.GetStringAsync(article.ToString(), cancellationToken);
            if (productString != null)
            {
                product = JsonSerializer.Deserialize<Product>(productString);
            }
            return product;
        }

        public async Task RemoveAsync(int article, CancellationToken cancellationToken)
        {
            string? productString = await _cache.GetStringAsync(article.ToString(), cancellationToken);
            if (productString != null)
                await _cache.RemoveAsync(article.ToString(), cancellationToken);
        }

        public async Task SaveAsync(Product product, CancellationToken cancellationToken)
        {
            string? productString = await _cache.GetStringAsync(product.Article.ToString(), cancellationToken);
            if (productString == null)
                await _cache.SetStringAsync(product.Article.ToString(), JsonSerializer.Serialize(product), _cacheOptions, cancellationToken);
        }

        public async Task UpdateAsync(Product product, CancellationToken cancellationToken)
        {
            await RemoveAsync(product.Article, cancellationToken);
            await SaveAsync(product, cancellationToken);
        }
    }
}

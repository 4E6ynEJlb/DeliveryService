using Domain.Models.ApplicationModels;
using Domain.Models.Entities.SQLEntities;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Infrastructure
{
    public class UserCacheService:IUserCacheService
    {
        private readonly IDistributedCache _cache;
        private readonly DistributedCacheEntryOptions _cacheOptions;
        public UserCacheService(IDistributedCache cache, IOptions<RepositoryOptions> repositoryOptions)
        {
            _cache = cache;
            _cacheOptions = new DistributedCacheEntryOptions() { SlidingExpiration = TimeSpan.FromMinutes(repositoryOptions.Value.CacheExpirationMins) };
        }
        public async Task<User?> GetAsync(Guid id, CancellationToken cancellationToken)
        {
            User? user = null;
            string? userString = await _cache.GetStringAsync(id.ToString(), cancellationToken);
            if (userString != null)
            {
                user = JsonSerializer.Deserialize<User>(userString);
            }
            return user;
        }

        public async Task RemoveAsync(Guid id, CancellationToken cancellationToken)
        {
            string? userString = await _cache.GetStringAsync(id.ToString(), cancellationToken);
            if (userString != null)
                await _cache.RemoveAsync(id.ToString(), cancellationToken);
        }

        public async Task SaveAsync(User user, CancellationToken cancellationToken)
        {
            string? userString = await _cache.GetStringAsync(user.Id.ToString(), cancellationToken);
            if (userString == null)
                await _cache.SetStringAsync(user.Id.ToString(), JsonSerializer.Serialize(user), _cacheOptions, cancellationToken);
        }

        public async Task UpdateAsync(User user, CancellationToken cancellationToken)
        {
            await RemoveAsync(user.Id, cancellationToken);
            await SaveAsync(user, cancellationToken);
        }
    }
}

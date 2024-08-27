using Domain.Models.Entities.SQLEntities;

namespace Infrastructure.Interfaces
{
    public interface IUserCacheService
    {
        public Task<User?> GetAsync(Guid id, CancellationToken cancellationToken);
        public Task SaveAsync(User user, CancellationToken cancellationToken);
        public Task RemoveAsync(Guid id, CancellationToken cancellationToken);
        public Task UpdateAsync(User user, CancellationToken cancellationToken);
    }
}

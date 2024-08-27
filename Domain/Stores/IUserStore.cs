using Domain.Models.Entities.SQLEntities;
using Domain.Models.VievModels;

namespace Domain.Stores
{
    public interface IUserStore
    {
        public Task<User> GetUserByIdAsync(Guid id, CancellationToken cancellationToken);
        public Task<User> GetUserByAuthAsync(AuthModel authModel, CancellationToken cancellationToken);
        public Task<List<string>> GetBirthdayPeopleTelegramAsync(CancellationToken cancellationToken);
        public Task AddUserAsync(User user, CancellationToken cancellationToken);
        public Task RemoveUserAsync(Guid id, CancellationToken cancellationToken);
        public Task<bool> EditUserTelegramAsync(Guid id, string newTelegramId, CancellationToken cancellationToken);
        public Task AddUserBirthDateAsync(Guid id, DateOnly birthDate, CancellationToken cancellationToken);
        public Task DebitBonusesAsync(Guid id, decimal amount, CancellationToken cancellationToken);
        public Task EditUserAuthAsync(Guid id, AuthModel newAuth, CancellationToken cancellationToken);
        public Task AssignAsAdminAsync(Guid id, CancellationToken cancellationToken);
        public Task UnassignAsAdminAsync(Guid id, CancellationToken cancellationToken);
    }
}

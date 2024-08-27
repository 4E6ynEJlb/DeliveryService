using Domain.Models.VievModels;

namespace Application.Interfaces
{
    public interface IUserUserService
    {
        public Task<UserOutputModel> GetUserByIdAsync(Guid id, CancellationToken cancellationToken);
        public Task DeleteUserAsync(Guid id, CancellationToken cancellationToken);
        public Task EditUserTelegramAsync(Guid id, string newTelegramId, CancellationToken cancellationToken);
        public Task AddUserBirthDateAsync(Guid id, DateOnly birthDate, CancellationToken cancellationToken);
        public Task EditUserAuthAsync(Guid id, AuthModel newAuth, CancellationToken cancellationToken);
    }
}

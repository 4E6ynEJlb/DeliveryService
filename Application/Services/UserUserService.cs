using Application.Interfaces;
using Domain.Models.ApplicationModels;
using Domain.Models.VievModels;
using Domain.Stores;
using Microsoft.Extensions.Options;

namespace Application.Services
{
    public class UserUserService : IUserUserService
    {
        private readonly IUserStore _userStore;
        private readonly decimal _bonusesForTelegram;
        private readonly decimal _bonusesForBirthDate;
        public UserUserService(IUserStore userStore, IOptions<ServicesOptions> servicesOptions) 
        {
            _userStore = userStore;
            _bonusesForTelegram = servicesOptions.Value.BonusesForTelegram;
            _bonusesForBirthDate = servicesOptions.Value.BonusesForBirthdate;
        }
        public async Task<UserOutputModel> GetUserByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return new UserOutputModel(await _userStore.GetUserByIdAsync(id, cancellationToken));
        }
        public async Task DeleteUserAsync(Guid id, CancellationToken cancellationToken)
        {
            await _userStore.RemoveUserAsync(id, cancellationToken);
        }

        public async Task EditUserAuthAsync(Guid id, AuthModel newAuth, CancellationToken cancellationToken)
        {
            await _userStore.EditUserAuthAsync(id, newAuth, cancellationToken);
        }

        public async Task AddUserBirthDateAsync(Guid id, DateOnly birthDate, CancellationToken cancellationToken)
        {
            await _userStore.AddUserBirthDateAsync(id, birthDate, cancellationToken);
            await _userStore.DebitBonusesAsync(id, _bonusesForBirthDate, cancellationToken);
        }

        public async Task EditUserTelegramAsync(Guid id, string newTelegramId, CancellationToken cancellationToken)
        {
            if(!await _userStore.EditUserTelegramAsync(id, newTelegramId, cancellationToken))
                await _userStore.DebitBonusesAsync(id, _bonusesForTelegram, cancellationToken);
        }
    }
}

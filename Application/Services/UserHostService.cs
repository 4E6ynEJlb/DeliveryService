using Application.Interfaces;
using Domain.Stores;

namespace Application.Services
{
    public class UserHostService : IUserHostService
    {
        private readonly IUserStore _userStore;
        public UserHostService(IUserStore userStore)
        {
            _userStore = userStore;
        }
        public async Task AssignUserAsAdminAsync(Guid id, CancellationToken cancellationToken)
        {
            await _userStore.AssignAsAdminAsync(id, cancellationToken);
        }

        public async Task DebitBonusesAsync(Guid id, decimal amount, CancellationToken cancellationToken)
        {
            await _userStore.DebitBonusesAsync(id, amount, cancellationToken);
        }

        public async Task UnassignUserAsAdminAsync(Guid id, CancellationToken cancellationToken)
        {
            await _userStore.UnassignAsAdminAsync(id, cancellationToken);
        }
    }
}

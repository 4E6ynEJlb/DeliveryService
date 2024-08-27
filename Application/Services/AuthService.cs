using Application.Interfaces;
using Domain.Models.ApplicationModels;
using Domain.Models.Entities.SQLEntities;
using Domain.Models.VievModels;
using Domain.Stores;
using Microsoft.Extensions.Options;

namespace Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly HostAuthOptions _hostAuth;
        private readonly IUserStore _userStore;
        private readonly IRestaurantStore _restaurantStore;
        private readonly decimal _bonusesForTelegram;
        private readonly decimal _bonusesForBirthDate;
        public AuthService(IUserStore userStore, IRestaurantStore restaurantStore, IOptions<HostAuthOptions> hostAuth, IOptions<ServicesOptions> servicesOptions)
        {
            _hostAuth = hostAuth.Value;
            _userStore = userStore;
            _restaurantStore = restaurantStore;
            _bonusesForTelegram = servicesOptions.Value.BonusesForTelegram;
            _bonusesForBirthDate = servicesOptions.Value.BonusesForBirthdate;
        }

        public async Task<Restaurant> GetRestaurantByAuthAsync(AuthModel authModel, CancellationToken cancellationToken)
        {
            return await _restaurantStore.GetRestaurantByAuthAsync(authModel, cancellationToken);
        }

        public async Task<UserOutputModel> GetUserByAuthAsync(AuthModel authModel, CancellationToken cancellationToken)
        {
            User user = await _userStore.GetUserByAuthAsync(authModel, cancellationToken);
            return new UserOutputModel(user);
        }
        public bool AuthHost(AuthModel authModel)
        {
            if (_hostAuth.Login != authModel.Login || _hostAuth.Password != authModel.Password)
                return false;
            return true;
        }

        public async Task RegisterUserAsync(UserRegisterModel userRegisterModel, CancellationToken cancellationToken)
        {
            User user = userRegisterModel.ToUser();
            if (userRegisterModel.BirthDate.HasValue)
                user.Bonuses += _bonusesForBirthDate;
            if (userRegisterModel.TelegramId != null)
                user.Bonuses += _bonusesForTelegram;
            await _userStore.AddUserAsync(user, cancellationToken);
        }
    }
}

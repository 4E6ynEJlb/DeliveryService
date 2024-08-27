using Domain.Models.Entities.SQLEntities;
using Domain.Models.VievModels;

namespace Application.Interfaces
{
    public interface IAuthService
    {
        public Task<Restaurant> GetRestaurantByAuthAsync(AuthModel authModel, CancellationToken cancellationToken);
        public Task<UserOutputModel> GetUserByAuthAsync(AuthModel authModel, CancellationToken cancellationToken);
        public Task RegisterUserAsync(UserRegisterModel userRegisterModel, CancellationToken cancellationToken);
        public bool AuthHost(AuthModel authModel);
    }
}

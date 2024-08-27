using Domain.Models.Entities.SQLEntities;
using Domain.Models.VievModels;

namespace Domain.Stores
{
    public interface IRestaurantStore
    {
        public Task<Restaurant> GetRestaurantByAuthAsync(AuthModel authModel, CancellationToken cancellationToken);
        public Task<string[]> GetRestaurantsInCityAdressesAsync(string city, CancellationToken cancellationToken);
        public Task AddRestaurantAsync(Restaurant restaurant, CancellationToken cancellationToken);
        public Task RemoveRestaurantAsync(string adress, CancellationToken cancellationToken);
        public Task EditRestaurantAuthAsync(string adress, AuthModel authModel, CancellationToken cancellationToken);
    }
}

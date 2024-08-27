using Application.Interfaces;
using Domain.Stores;

namespace Application.Services
{
    public class RestaurantUserService : IRestaurantUserService
    {
        private readonly IRestaurantStore _restaurantStore;
        public RestaurantUserService(IRestaurantStore restaurantStore) 
        {
            _restaurantStore = restaurantStore;
        }
        public async Task<string[]> GetRestaurantsInCityAdressesAsync(string city, CancellationToken cancellationToken)
        {
            return await _restaurantStore.GetRestaurantsInCityAdressesAsync(city, cancellationToken);
        }
    }
}

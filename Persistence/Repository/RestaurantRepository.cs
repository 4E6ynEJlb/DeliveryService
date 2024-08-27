using Domain.Models.Entities.SQLEntities;
using Domain.Models.VievModels;
using Domain.Stores;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repository
{
    public class RestaurantRepository : IRestaurantStore
    {
        private readonly SQLContext _context;
        public RestaurantRepository(SQLContext context)
        {
            _context = context;
        }
        public async Task AddRestaurantAsync(Restaurant restaurant, CancellationToken cancellationToken)
        {
            _context.Restaurants.Add(restaurant);
            await _context.SaveChangesAsync();
        }

        public async Task<string[]> GetRestaurantsInCityAdressesAsync(string city, CancellationToken cancellationToken)
        {
            return await _context.Restaurants.Where(r => EF.Functions.Like(r.Adress, $"%{city}%")).Select(r=>r.Adress).ToArrayAsync(cancellationToken);
        }

        public async Task<Restaurant> GetRestaurantByAuthAsync(AuthModel authModel, CancellationToken cancellationToken)
        {
            Restaurant? restaurant = await _context.Restaurants.AsNoTracking().FirstOrDefaultAsync(r => r.Login == authModel.Login && r.Password == authModel.Password, cancellationToken);
            if (restaurant == null)
                throw new DoesNotExistException(typeof(Restaurant));
            return restaurant;
        }

        public async Task RemoveRestaurantAsync(string adress, CancellationToken cancellationToken)
        {
            Restaurant? restaurant = await _context.Restaurants.AsNoTracking().FirstOrDefaultAsync(r => r.Adress.Equals(adress), cancellationToken);
            if (restaurant == null)
                throw new DoesNotExistException(typeof(Restaurant));
            _context.Restaurants.Remove(restaurant);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task EditRestaurantAuthAsync(string adress, AuthModel authModel, CancellationToken cancellationToken)
        {
            Restaurant? restaurant = await _context.Restaurants.FirstOrDefaultAsync(r => r.Adress.Equals(adress), cancellationToken);
            if (restaurant == null)
                throw new DoesNotExistException(typeof(Restaurant));
            restaurant.Login = authModel.Login;
            restaurant.Password = authModel.Password;
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}

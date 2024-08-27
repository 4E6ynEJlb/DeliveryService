namespace Application.Interfaces
{
    public interface IRestaurantUserService
    {
        public Task<string[]> GetRestaurantsInCityAdressesAsync(string city, CancellationToken cancellationToken);
    }
}

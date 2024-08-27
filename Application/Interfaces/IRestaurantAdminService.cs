using Domain.Models.Entities.SQLEntities;
using Domain.Models.VievModels;

namespace Application.Interfaces
{
    public interface IRestaurantAdminService
    {
        public Task AddRestaurantAsync(Restaurant restaurant, Guid admin, CancellationToken cancellationToken);
        public Task RemoveRestaurantAsync(string adress, Guid admin, CancellationToken cancellationToken);
        public Task EditRestaurantAuthAsync(string adress, AuthModel authModel, Guid admin, CancellationToken cancellationToken);
    }
}

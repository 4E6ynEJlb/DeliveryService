using Domain.Models.Entities.SQLEntities;
using Domain.Models.VievModels;

namespace Domain.Stores
{
    public interface IProductStore
    {
        public Task<Product> GetProductAsync(int article, CancellationToken cancellationToken);
        public Task<Product[]> GetVisibleGoodsArrayAsync(int page, int pageSize, GoodsListOptionsModel listOptions, CancellationToken cancellationToken);
        public Task<Product[]> GetInvisibleGoodsArrayAsync(int page, int pageSize, string? textInTitle, CancellationToken cancellationToken);
        public Task AddProductAsync(Product product, CancellationToken cancellationToken);
        public Task<string?> RemoveProductAsync(int article, CancellationToken cancellationToken);
        public Task EditPriceAsync(int article, decimal price, CancellationToken cancellationToken);
        public Task UpdateCookingTimeAsync(int article, TimeOnly lastCookingTime, CancellationToken cancellationToken);
        public Task UpdateRatingAsync(int article, int mark, CancellationToken cancellationToken);
        public Task ShowAsync(int article, CancellationToken cancellationToken);
        public Task HideAsync(int article, CancellationToken cancellationToken);
        public Task AttachImageAsync(string imageName, int article, CancellationToken cancellationToken);
        public Task<string?> DetachImageAsync(int article, CancellationToken cancellationToken);
    }
}

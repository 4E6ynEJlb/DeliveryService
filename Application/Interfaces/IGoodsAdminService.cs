using Domain.Models.VievModels;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces
{
    public interface IGoodsAdminService
    {
        public Task<ProductOutputModel[]> GetInvisibleGoodsArrayAsync(int page, int pageSize, string? textInTitle, CancellationToken cancellationToken);
        public Task AddProductAsync(ProductInputModel product, Guid admin, CancellationToken cancellationToken);
        public Task RemoveProductAsync(int article, Guid admin, CancellationToken cancellationToken);
        public Task EditPriceAsync(int article, decimal price, Guid admin, CancellationToken cancellationToken);
        public Task ShowProductAsync(int article, Guid admin, CancellationToken cancellationToken);
        public Task HideProductAsync(int article, Guid admin, CancellationToken cancellationToken);
        public Task AttachImageAsync(IFormFile file, int article, Guid admin, CancellationToken cancellationToken);
        public Task DetachImageAsync(int article, Guid admin, CancellationToken cancellationToken);
    }
}
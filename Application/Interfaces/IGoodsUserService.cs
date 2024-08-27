using Domain.Models.VievModels;

namespace Application.Interfaces
{
    public interface IGoodsUserService
    {
        public Task<ProductOutputModel> GetProductAsync(int article, CancellationToken cancellationToken);
        public Task<ProductOutputModel[]> GetVisibleGoodsArrayAsync(int page, int pageSize, GoodsListOptionsModel listOptions, CancellationToken cancellationToken);
        public Task<List<ProductOutputModel>> GetHotGoodsListAsync(int goodsCount, CancellationToken cancellationToken);
        public Task RateProductAsync(int article, int mark, CancellationToken cancellationToken);
    }
}

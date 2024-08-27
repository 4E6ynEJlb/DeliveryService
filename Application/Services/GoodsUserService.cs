using Application.Exceptions;
using Application.Interfaces;
using Domain.Models.ApplicationModels;
using Domain.Models.Entities.SQLEntities;
using Domain.Models.VievModels;
using Domain.Stores;
using Microsoft.Extensions.Options;

namespace Application.Services
{
    public class GoodsUserService : IGoodsUserService
    {
        private readonly IProductStore _productStore;
        private readonly ISoldProductStore _soldProductStore;
        private readonly string _link;
        public GoodsUserService(IProductStore productStore, ISoldProductStore soldProductStore, IOptions<ServicesOptions> options)
        {
            _productStore = productStore;
            _soldProductStore = soldProductStore;
            _link = options.Value.GoodsImagesLinkTemplate;
        }
        public async Task<ProductOutputModel> GetProductAsync(int article, CancellationToken cancellationToken)
        {
            return new ProductOutputModel(await _productStore.GetProductAsync(article, cancellationToken), _link);
        }
        public async Task<List<ProductOutputModel>> GetHotGoodsListAsync(int goodsCount, CancellationToken cancellationToken)
        {
            List<int> articles = await _soldProductStore.GetHotArticleListAsync(goodsCount, cancellationToken);
            List<ProductOutputModel> pOMList = new List<ProductOutputModel>();
            foreach (int article in articles)
            {
                pOMList.Add(new ProductOutputModel(await _productStore.GetProductAsync(article, cancellationToken), _link));
            }
            return pOMList;
        }
        public async Task<ProductOutputModel[]> GetVisibleGoodsArrayAsync(int page, int pageSize, GoodsListOptionsModel listOptions, CancellationToken cancellationToken)
        {
            Product[] productArray = await _productStore.GetVisibleGoodsArrayAsync(page, pageSize, listOptions, cancellationToken);
            ProductOutputModel[] productOutputModelArray = new ProductOutputModel[productArray.Length];
            for (int i = 0; i < productArray.Length; i++)
            {
                productOutputModelArray[i] = new ProductOutputModel(productArray[i], _link);
            }
            return productOutputModelArray;
        }
        public async Task RateProductAsync(int article, int mark, CancellationToken cancellationToken)
        {
            if (mark < 0 || mark > 5)
                throw new InvalidMarkException();
            await _productStore.UpdateRatingAsync(article, mark, cancellationToken);
        }
    }
}

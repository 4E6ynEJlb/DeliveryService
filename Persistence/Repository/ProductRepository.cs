using Domain.Models.ApplicationModels;
using Domain.Models.Entities.SQLEntities;
using Domain.Models.VievModels;
using Domain.Stores;
using Microsoft.EntityFrameworkCore;
using Persistence.Exceptions;
using Infrastructure;
using Infrastructure.Interfaces;

namespace Persistence.Repository
{
    public class ProductRepository : IProductStore
    {
        private readonly SQLContext _context;
        private readonly IProductCacheService _cacheService;
        public ProductRepository(SQLContext context, IProductCacheService cacheService) 
        {
            _context = context;
            _cacheService = cacheService;
        }
        public async Task<Product> GetProductAsync(int article, CancellationToken cancellationToken)
        {
            Product? product = await _cacheService.GetAsync(article, cancellationToken);
            if (product == null)
                product = await _context.Goods.AsNoTracking().FirstOrDefaultAsync(p => p.Article == article, cancellationToken);
            if (product == null)
                throw new DoesNotExistException(typeof(Product));
            await _cacheService.SaveAsync(product, cancellationToken);
            return product;
        }

        public async Task<Product[]> GetVisibleGoodsArrayAsync(int page, int pageSize, GoodsListOptionsModel listOptions, CancellationToken cancellationToken)
        {
            IQueryable<Product> goodsQuery = _context.Goods.Where(p => p.Visible);
            if (listOptions.TextInTitle != null)
                goodsQuery = goodsQuery.Where(p => EF.Functions.Like(p.Title, $"%{listOptions.TextInTitle}%"));
            switch (listOptions.Criterium)
            {
                case SortCriteria.Price:
                    goodsQuery = (listOptions.IsAsc ? goodsQuery.OrderBy(p => p.Price) : goodsQuery.OrderByDescending(p => p.Price));
                    break;
                case SortCriteria.Rating:
                    goodsQuery = (listOptions.IsAsc ? goodsQuery.OrderBy(p => p.Rating) : goodsQuery.OrderByDescending(p => p.Rating));
                    break;
                case SortCriteria.CookingTime:
                    goodsQuery = (listOptions.IsAsc ? goodsQuery.OrderBy(p => p.AverageCookingTime) : goodsQuery.OrderByDescending(p => p.AverageCookingTime));
                    break;
            }
            int pagesCount = await goodsQuery.CountAsync(cancellationToken);
            if (pagesCount > 0)
                pagesCount = pagesCount / pageSize + ((pagesCount % pageSize) == 0 ? 0 : 1);
            else
                pagesCount = 1;
            if (page > pagesCount || page < 1)
                throw new InvalidPageException();
            return await goodsQuery.Skip((page - 1) * pageSize).Take(pageSize).ToArrayAsync(cancellationToken);
        }

        public async Task<Product[]> GetInvisibleGoodsArrayAsync(int page, int pageSize, string? textInTitle, CancellationToken cancellationToken)
        {
            IQueryable<Product> goodsQuery = _context.Goods.Where(p => !p.Visible);
            if (textInTitle != null)
                goodsQuery = goodsQuery.Where(p => EF.Functions.Like(p.Title, $"%{textInTitle}%"));
            int pagesCount = await goodsQuery.CountAsync(cancellationToken);
            if (pagesCount > 0)
                pagesCount = pagesCount / pageSize + ((pagesCount % pageSize) == 0 ? 0 : 1);
            else
                pagesCount = 1;
            if (page > pagesCount || page < 1)
                throw new InvalidPageException();
            return await goodsQuery.Skip((page - 1) * pageSize).Take(pageSize).ToArrayAsync(cancellationToken);
        }
        public async Task AddProductAsync(Product product, CancellationToken cancellationToken)
        {
            _context.Goods.Add(product);
            await _context.SaveChangesAsync(cancellationToken);
            await _cacheService.SaveAsync(product, cancellationToken);
        }

        public async Task EditPriceAsync(int article, decimal price, CancellationToken cancellationToken)
        {
            Product? product = await _cacheService.GetAsync(article, cancellationToken);
            if (product == null)            
                product = await _context.Goods.AsNoTracking().FirstOrDefaultAsync(p => p.Article == article);
            if (product == null)
                throw new DoesNotExistException(typeof(Product));
            product.Price = price;
            _context.Goods.Update(product);
            await _context.SaveChangesAsync(cancellationToken);
            await _cacheService.UpdateAsync(product, cancellationToken);
        }

        public async Task HideAsync(int article, CancellationToken cancellationToken)
        {
            Product? product = await _cacheService.GetAsync(article, cancellationToken);
            if (product == null)
                product = await _context.Goods.AsNoTracking().FirstOrDefaultAsync(p => p.Article == article);
            if (product == null)
                throw new DoesNotExistException(typeof(Product));
            product.Visible = false;
            _context.Goods.Update(product);
            await _context.SaveChangesAsync(cancellationToken);
            await _cacheService.UpdateAsync(product, cancellationToken);
        }

        public async Task<string?> RemoveProductAsync(int article, CancellationToken cancellationToken)
        {
            Product? product = await _cacheService.GetAsync(article, cancellationToken);
            if (product == null)            
                product = await _context.Goods.AsNoTracking().FirstOrDefaultAsync(p => p.Article == article, cancellationToken);
            if (product == null)
                throw new DoesNotExistException(typeof(Product));
            _context.Goods.Remove(product);
            await _context.SaveChangesAsync(cancellationToken);
            await _cacheService.RemoveAsync(article, cancellationToken);
            return product.ImageName;
        }

        public async Task ShowAsync(int article, CancellationToken cancellationToken)
        {
            Product? product = await _cacheService.GetAsync(article, cancellationToken);
            if (product == null)
                product = await _context.Goods.AsNoTracking().FirstOrDefaultAsync(p => p.Article == article);
            if (product == null)
                throw new DoesNotExistException(typeof(Product));
            product.Visible = true;
            _context.Goods.Update(product);
            await _context.SaveChangesAsync();
            await _cacheService.UpdateAsync(product, cancellationToken);
        }

        public async Task UpdateCookingTimeAsync(int article, TimeOnly lastCookingTime, CancellationToken cancellationToken)
        {
            Product? product = await _cacheService.GetAsync(article, cancellationToken);
            if (product == null)
                product = await _context.Goods.AsNoTracking().FirstOrDefaultAsync(p => p.Article == article);
            if (product == null)
                throw new DoesNotExistException(typeof(Product));
            if (!product.AverageCookingTime.HasValue)
            {
                product.AverageCookingTime = lastCookingTime;
                product.AlreadyCooked = 1;
            }
            int alreadyCooked = product.AlreadyCooked;
            long totalTicks = product.AverageCookingTime.Value.Ticks * alreadyCooked + lastCookingTime.Ticks;
            product.AverageCookingTime = new TimeOnly(totalTicks / ++alreadyCooked);
            product.AlreadyCooked = alreadyCooked;
            _context.Goods.Update(product);
            await _context.SaveChangesAsync(cancellationToken);
            await _cacheService.UpdateAsync(product, cancellationToken);
        }

        public async Task UpdateRatingAsync(int article, int mark, CancellationToken cancellationToken)
        {
            Product? product = await _cacheService.GetAsync(article, cancellationToken);
            if (product == null)
                product = await _context.Goods.AsNoTracking().FirstOrDefaultAsync(p => p.Article == article);
            if (product == null)
                throw new DoesNotExistException(typeof(Product));
            if (!product.Rating.HasValue)
            {
                product.Rating = mark;
                product.AlreadyRated = 1;
            }
            int alreadyRated = product.AlreadyRated;
            float totalRating = product.Rating.Value * alreadyRated + mark;
            product.Rating = totalRating / ++alreadyRated;
            product.AlreadyRated = alreadyRated;
            _context.Goods.Update(product);
            await _context.SaveChangesAsync(cancellationToken);
            await _cacheService.UpdateAsync(product, cancellationToken);
        }

        public async Task AttachImageAsync(string imageName, int article, CancellationToken cancellationToken)
        {
            Product? product = await _cacheService.GetAsync(article, cancellationToken);
            if (product == null)
                product = await _context.Goods.AsNoTracking().FirstOrDefaultAsync(p => p.Article == article);
            if (product == null)
                throw new DoesNotExistException(typeof(Product));
            product.ImageName = imageName;
            _context.Goods.Update(product);
            await _context.SaveChangesAsync(cancellationToken);
            await _cacheService.UpdateAsync(product, cancellationToken);
        }

        public async Task<string?> DetachImageAsync(int article, CancellationToken cancellationToken)
        {
            Product? product = await _cacheService.GetAsync(article, cancellationToken);
            if (product == null)
                product = await _context.Goods.AsNoTracking().FirstOrDefaultAsync(p => p.Article == article);
            if (product == null)
                throw new DoesNotExistException(typeof(Product));
            string? oldImageName = product.ImageName;
            product.ImageName = null;
            _context.Goods.Update(product);
            await _context.SaveChangesAsync(cancellationToken);
            await _cacheService.UpdateAsync(product, cancellationToken);
            return oldImageName;
        }
    }
}
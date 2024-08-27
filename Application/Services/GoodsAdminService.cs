using Application.Exceptions;
using Application.Interfaces;
using Domain.Models.ApplicationModels;
using Domain.Models.Entities.MongoDBEntities;
using Domain.Models.Entities.SQLEntities;
using Domain.Models.VievModels;
using Domain.Stores;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Application.Services
{
    public class GoodsAdminService : IGoodsAdminService
    {
        private readonly IAuditLogStore _auditLogStore;
        private readonly IProductStore _productStore;
        private readonly IFileClient _fileClient;
        private readonly string _link;
        public GoodsAdminService(IProductStore productStore, IAuditLogStore auditLogStore, IFileClient fileClient, IOptions<ServicesOptions> options) 
        {
            _productStore = productStore;
            _auditLogStore = auditLogStore;
            _link = options.Value.GoodsImagesLinkTemplate;
            _fileClient = fileClient;
        }
        public async Task<ProductOutputModel[]> GetInvisibleGoodsArrayAsync(int page, int pageSize, string? textInTitle, CancellationToken cancellationToken)
        {
            Product[] productArray = await _productStore.GetInvisibleGoodsArrayAsync(page, pageSize, textInTitle, cancellationToken);
            ProductOutputModel[] productOutputModelArray = new ProductOutputModel[productArray.Length];
            for (int i = 0; i<productArray.Length; i++)
            {
                productOutputModelArray[i] = new ProductOutputModel(productArray[i], _link);
            }
            return productOutputModelArray;
        }
        public async Task AddProductAsync(ProductInputModel productInputModel, Guid adminId, CancellationToken cancellationToken)
        {
            await _productStore.AddProductAsync(productInputModel.ToProduct(), cancellationToken);
            await _auditLogStore.AddRecordAsync(new AuditLogRecord(adminId, $"{AuditLogExpressions.ARTICLE_ADDED}{productInputModel.Article}"), cancellationToken);
        }

        public async Task AttachImageAsync(IFormFile file, int article, Guid adminId, CancellationToken cancellationToken)
        {
            if (!file.ContentType.Contains("image"))
                throw new InvalidFileFormatException();
            Product product = await _productStore.GetProductAsync(article, cancellationToken);
            if (product.ImageName != null)
            {
                await _fileClient.DeleteAsync(product.ImageName, cancellationToken);
                await _auditLogStore.AddRecordAsync(new AuditLogRecord(adminId, $"{AuditLogExpressions.IMAGE_REMOVED}{article}"), cancellationToken);
            }
            Stream stream = file.OpenReadStream();
            await _fileClient.SaveAsync(stream, file.FileName, cancellationToken);            
            await _productStore.AttachImageAsync(file.FileName, article, cancellationToken);
            await _auditLogStore.AddRecordAsync(new AuditLogRecord(adminId, $"{AuditLogExpressions.IMAGE_ADDED}{article}"), cancellationToken);
        }

        public async Task DetachImageAsync(int article, Guid adminId, CancellationToken cancellationToken)
        {
            string? imgName = await _productStore.DetachImageAsync(article, cancellationToken);
            if (imgName != null) 
            {
                await _fileClient.DeleteAsync(imgName, cancellationToken);
            }
            await _auditLogStore.AddRecordAsync(new AuditLogRecord(adminId, $"{AuditLogExpressions.IMAGE_REMOVED}{article}"), cancellationToken);
        }

        public async Task EditPriceAsync(int article, decimal price, Guid adminId, CancellationToken cancellationToken)
        {
            await _productStore.EditPriceAsync(article, price, cancellationToken);
            await _auditLogStore.AddRecordAsync(new AuditLogRecord(adminId, $"{AuditLogExpressions.PRICE_CHANGED}{article}"), cancellationToken);
        }

        public async Task HideProductAsync(int article, Guid adminId, CancellationToken cancellationToken)
        {
            await _productStore.HideAsync(article, cancellationToken);
            await _auditLogStore.AddRecordAsync(new AuditLogRecord(adminId, $"{AuditLogExpressions.ARTICLE_HIDDEN}{article}"), cancellationToken);
        }

        public async Task RemoveProductAsync(int article, Guid adminId, CancellationToken cancellationToken)
        {
            string? imgName = await _productStore.RemoveProductAsync(article, cancellationToken);
            if (imgName != null)
            {
                await _fileClient.DeleteAsync(imgName, cancellationToken);
            }
            await _auditLogStore.AddRecordAsync(new AuditLogRecord(adminId, $"{AuditLogExpressions.ARTICLE_REMOVED}{article}"), cancellationToken);
        }

        public async Task ShowProductAsync(int article, Guid adminId, CancellationToken cancellationToken)
        {
            await _productStore.ShowAsync(article, cancellationToken);
            await _auditLogStore.AddRecordAsync(new AuditLogRecord(adminId, $"{AuditLogExpressions.ARTICLE_SHOWN}{article}"), cancellationToken);
        }
    }
}

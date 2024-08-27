using Domain.Models.ApplicationModels;
using Domain.Models.Entities.MongoDBEntities;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Infrastructure
{
    public class MongoContext : IMongoContext
    {
        private readonly MongoDBOptions _mongoOptions;
        private readonly IMongoDatabase _database;
        public IMongoCollection<SoldProduct> SoldGoods
        {
            get
            {
                return _database.GetCollection<SoldProduct>(_mongoOptions.SoldGoodsCollectionName);
            }
        }
        public IMongoCollection<Report> Reports
        {
            get
            {
                return _database.GetCollection<Report>(_mongoOptions.ReportsCollectionName);
            }
        }
        public IMongoCollection<Order> Orders
        {
            get
            {
                return _database.GetCollection<Order>(_mongoOptions.OrdersCollectionName);
            }
        }
        public IMongoCollection<AuditLogRecord> AuditRecords
        {
            get
            {
                return _database.GetCollection<AuditLogRecord>(_mongoOptions.AuditRecordsCollectionName);
            }
        }
        public MongoContext(IOptions<MongoDBOptions> mongoOptions, IOptions<RepositoryOptions> repositoryOptions, ILogger<MongoContext> logger)
        {
            _mongoOptions = mongoOptions.Value;
            var client = new MongoClient(_mongoOptions.MongoDefaultConnection);
            if (client == null)
            {
                logger.LogCritical("Mongo Client not found");
                throw new NullReferenceException("Mongo Client not found"); 
            }
            logger.LogInformation($"Connected Mongo Client on: {_mongoOptions.MongoDefaultConnection}");
            _database = client.GetDatabase(_mongoOptions.DBName);
            logger.LogInformation($"Connected Mongo Database: {_mongoOptions.DBName}");
            _database.CreateCollection(_mongoOptions.SoldGoodsCollectionName);
            _database.CreateCollection(_mongoOptions.ReportsCollectionName);
            _database.CreateCollection(_mongoOptions.OrdersCollectionName);
            _database.CreateCollection(_mongoOptions.AuditRecordsCollectionName);
            SoldGoods.Indexes.CreateOne(new CreateIndexModel<SoldProduct>
            (
                keys: Builders<SoldProduct>.IndexKeys.Ascending(p => p.ExpireAt),
                options: new CreateIndexOptions()
                {
                    ExpireAfter = TimeSpan.FromHours(repositoryOptions.Value.HotGoodsExpirationHours),
                    Name = "ExpireAtIndex"
                }
            ));
            AuditRecords.Indexes.CreateOne(new CreateIndexModel<AuditLogRecord>
            (
                keys: Builders<AuditLogRecord>.IndexKeys.Ascending(r => r.ExpireAt),
                options: new CreateIndexOptions()
                {
                    ExpireAfter = TimeSpan.FromDays(repositoryOptions.Value.AuditExpirationDays),
                    Name = "ExpireAtIndex"
                }
            ));
        }
    }
}

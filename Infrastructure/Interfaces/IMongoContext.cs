using Domain.Models.Entities.MongoDBEntities;
using MongoDB.Driver;

namespace Infrastructure.Interfaces
{
    public interface IMongoContext
    {
        public IMongoCollection<SoldProduct> SoldGoods { get; }
        public IMongoCollection<Report> Reports { get; }
        public IMongoCollection<Order> Orders { get; }
        public IMongoCollection<AuditLogRecord> AuditRecords { get; }
    }
}

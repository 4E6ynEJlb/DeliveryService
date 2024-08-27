using Domain.Models.Entities.MongoDBEntities;
using Domain.Stores;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Microsoft.Extensions.Options;
using Domain.Models.ApplicationModels;
using Infrastructure.Interfaces;

namespace Persistence.Repository
{
    public class AuditLogRepository : IAuditLogStore
    {
        private readonly IMongoContext _context;
        private readonly int _auditExpirationDays;
        public AuditLogRepository(IMongoContext context, IOptions<RepositoryOptions> options) 
        {
            _context = context;
            _auditExpirationDays = options.Value.AuditExpirationDays;
        }

        public async Task<List<AuditLogRecord>> GetLastRecordsAsync(int count, CancellationToken cancellationToken)
        {
            return await _context.AuditRecords.AsQueryable().Take(count).ToListAsync(cancellationToken);
        }
        public async Task<int> GetRecordsCountAsync(CancellationToken cancellationToken)
        {
            return await _context.AuditRecords.AsQueryable().CountAsync(cancellationToken);
        }

        public async Task AddRecordAsync(AuditLogRecord record, CancellationToken cancellationToken)
        {
            record.Recorded = DateTime.Now;
            record.ExpireAt = record.Recorded.AddDays(_auditExpirationDays);
#pragma warning disable CS0618 // Тип или член устарел
            await _context.AuditRecords.InsertOneAsync(record, cancellationToken);
#pragma warning restore CS0618 // Тип или член устарел
        }
    }
}

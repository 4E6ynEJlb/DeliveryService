using Domain.Models.Entities.MongoDBEntities;

namespace Domain.Stores
{
    public interface IAuditLogStore
    {
        public Task<List<AuditLogRecord>> GetLastRecordsAsync(int count, CancellationToken cancellationToken);
        public Task<int> GetRecordsCountAsync(CancellationToken cancellationToken);
        public Task AddRecordAsync(AuditLogRecord record, CancellationToken cancellationToken);
    }
}

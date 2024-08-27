using Domain.Models.Entities.MongoDBEntities;

namespace Application.Interfaces
{
    public interface IAuditHostService
    {
        public Task<List<AuditLogRecord>> GetLastRecordsAsync(int count, CancellationToken cancellationToken);
        public Task<int> GetRecordsCountAsync(CancellationToken cancellationToken);
    }
}

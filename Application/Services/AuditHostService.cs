using Application.Interfaces;
using Domain.Models.Entities.MongoDBEntities;
using Domain.Stores;

namespace Application.Services
{
    public class AuditHostService : IAuditHostService
    {
        IAuditLogStore _auditLogStore;
        public AuditHostService(IAuditLogStore auditLogStore)
        {
            _auditLogStore = auditLogStore;
        }
        public async Task<List<AuditLogRecord>> GetLastRecordsAsync(int count, CancellationToken cancellationToken)
        {
            return await _auditLogStore.GetLastRecordsAsync(count, cancellationToken);
        }

        public async Task<int> GetRecordsCountAsync(CancellationToken cancellationToken)
        {
            return await _auditLogStore.GetRecordsCountAsync(cancellationToken);
        }
    }
}

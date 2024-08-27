using Application.Interfaces;
using Domain.Models.Entities.MongoDBEntities;
using Domain.Stores;

namespace Application.Services
{
    public class ReportAdminService : IReportAdminService
    {
        private readonly IReportStore _reportStore;
        private readonly IAuditLogStore _auditLogStore;
        public ReportAdminService(IReportStore reportStore, IAuditLogStore auditLogStore) 
        {
            _reportStore = reportStore;
            _auditLogStore = auditLogStore;
        }
        public async Task<Report> GetReportByIdAsync(DateTime id, CancellationToken cancellationToken)
        {
            return await _reportStore.GetReportByIdAsync(id, cancellationToken);
        }

        public async Task<List<DateTime>> GetReportsIdsAsync(CancellationToken cancellationToken)
        {
            return await _reportStore.GetReportsIdsAsync(cancellationToken);
        }

        public async Task RemoveReportAsync(DateTime id, Guid adminId, CancellationToken cancellationToken)
        {
            await _reportStore.RemoveReportAsync(id, cancellationToken);
            await _auditLogStore.AddRecordAsync(new AuditLogRecord(adminId, $"{AuditLogExpressions.REPORT_CLOSED}{id.ToString()}"), cancellationToken);
        }
    }
}

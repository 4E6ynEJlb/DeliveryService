using Domain.Models.Entities.MongoDBEntities;

namespace Application.Interfaces
{
    public interface IReportAdminService
    {
        public Task<Report> GetReportByIdAsync(DateTime id, CancellationToken cancellationToken);
        public Task<List<DateTime>> GetReportsIdsAsync(CancellationToken cancellationToken);
        public Task RemoveReportAsync(DateTime id, Guid adminId, CancellationToken cancellationToken);
    }
}

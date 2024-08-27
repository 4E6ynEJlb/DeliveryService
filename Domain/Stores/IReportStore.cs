using Domain.Models.Entities.MongoDBEntities;

namespace Domain.Stores
{
    public interface IReportStore
    {
        public Task<Report> GetReportByIdAsync(DateTime id, CancellationToken cancellationToken);
        public Task<List<DateTime>> GetReportsIdsAsync(CancellationToken cancellationToken);
        public Task AddReportAsync(Report report, CancellationToken cancellationToken);
        public Task RemoveReportAsync(DateTime id, CancellationToken cancellationToken);
    }
}

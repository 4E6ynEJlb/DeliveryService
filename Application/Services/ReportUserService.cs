using Application.Interfaces;
using Domain.Models.Entities.MongoDBEntities;
using Domain.Stores;

namespace Application.Services
{
    public class ReportUserService : IReportUserService
    {
        private readonly IReportStore _reportStore;
        public ReportUserService(IReportStore reportStore)
        {
            _reportStore = reportStore;
        }
        public async Task AddReportAsync(string message, Guid userId, CancellationToken cancellationToken)
        {
            await _reportStore.AddReportAsync(new Report() { Message = message, UserId = userId, Received = DateTime.Now}, cancellationToken);
        }
    }
}

using Domain.Models.Entities.MongoDBEntities;
using Domain.Stores;
using MongoDB.Driver.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using Infrastructure.Interfaces;
using Infrastructure;

namespace Persistence.Repository
{
    public class ReportRepository : IReportStore
    {
        private readonly IMongoContext _context;
        public ReportRepository(IMongoContext context) 
        {
            _context = context;
        }

        public async Task<Report> GetReportByIdAsync(DateTime id, CancellationToken cancellationToken)
        {
            Report? report = await _context.Reports.Find(new BsonDocument("Received", id)).FirstOrDefaultAsync(cancellationToken);
            if (report == null)
                throw new DoesNotExistException(typeof(Report));
            return report;
        }

        public async Task<List<DateTime>> GetReportsIdsAsync(CancellationToken cancellationToken)
        {
            return await _context.Reports.AsQueryable().Select(r => r.Received).ToListAsync(cancellationToken);
        }

        public async Task AddReportAsync(Report report, CancellationToken cancellationToken)
        {
            report.Received = DateTime.Now;
#pragma warning disable CS0618 // Тип или член устарел
            await _context.Reports.InsertOneAsync(report, cancellationToken);
#pragma warning restore CS0618 // Тип или член устарел
        }

        public async Task RemoveReportAsync(DateTime id, CancellationToken cancellationToken)
        {
            await _context.Reports.DeleteOneAsync(new BsonDocument("Received", id), cancellationToken);
        }
    }
}

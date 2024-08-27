namespace Application.Interfaces
{
    public interface IReportUserService
    {
        public Task AddReportAsync(string message, Guid userId, CancellationToken cancellationToken);
    }
}

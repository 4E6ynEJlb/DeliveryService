namespace Application.Interfaces
{
    public interface IUserHostService
    {
        public Task DebitBonusesAsync(Guid id, decimal amount, CancellationToken cancellationToken);
        public Task AssignUserAsAdminAsync(Guid id, CancellationToken cancellationToken);
        public Task UnassignUserAsAdminAsync(Guid id, CancellationToken cancellationToken);
    }
}

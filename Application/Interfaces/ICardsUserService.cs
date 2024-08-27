using Domain.Models.VievModels;

namespace Application.Interfaces
{
    public interface ICardsUserService
    {
        public Task<CardModel> GetCardByNumberAsync(string number, CancellationToken cancellationToken);
        public Task<CardModel[]> GetUserCardsAsync(Guid userId, CancellationToken cancellationToken);
        public Task AddCardAsync(CardModel card, Guid userId, CancellationToken cancellationToken);
        public Task RemoveCardAsync(string number, CancellationToken cancellationToken);
    }
}

using Domain.Models.Entities.SQLEntities;

namespace Domain.Stores
{
    public interface ICardStore
    {
        public Task<Card> GetCardByNumberAsync(string number, CancellationToken cancellationToken);
        public Task<Card[]> GetUserCardsAsync(Guid userId, CancellationToken cancellationToken);
        public Task AddCardAsync(Card card, CancellationToken cancellationToken);
        public Task RemoveCardAsync(string number, CancellationToken cancellationToken);
    }
}

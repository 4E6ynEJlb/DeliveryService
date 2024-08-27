using Domain.Models.Entities.SQLEntities;
using Domain.Stores;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repository
{
    public class CardRepository : ICardStore
    {
        private readonly SQLContext _context;
        public CardRepository(SQLContext context)
        {
            _context = context;
        }

        public async Task<Card> GetCardByNumberAsync(string number, CancellationToken cancellationToken)
        {
            Card? card = await _context.Cards.AsNoTracking().FirstOrDefaultAsync(u => u.Number.Equals(number), cancellationToken);
            if (card == null)
                throw new DoesNotExistException(typeof(Card));
            return card;
        }

        public async Task<Card[]> GetUserCardsAsync(Guid userId, CancellationToken cancellationToken)
        {
            User? user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
            if (user == null)
                throw new DoesNotExistException(typeof(User));
            return await _context.Cards.Where(c => c.UserId == userId).ToArrayAsync(cancellationToken);
        }

        public async Task AddCardAsync(Card card, CancellationToken cancellationToken)
        {
            _context.Cards.Add(card);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task RemoveCardAsync(string number, CancellationToken cancellationToken)
        {
            Card? card = await _context.Cards.AsNoTracking().FirstOrDefaultAsync(u => u.Number.Equals(number), cancellationToken);
            if (card == null)
                throw new DoesNotExistException(typeof(Card));
            _context.Cards.Remove(card);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}

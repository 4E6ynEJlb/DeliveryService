using Application.Interfaces;
using Domain.Models.ApplicationModels.Exceptions;
using Domain.Models.Entities.SQLEntities;
using Domain.Models.VievModels;
using Domain.Stores;

namespace Application.Services
{
    public class CardsUserService : ICardsUserService
    {
        ICardStore _cardStore;
        public CardsUserService(ICardStore cardStore)
        {
            _cardStore = cardStore;
        }

        public async Task<CardModel> GetCardByNumberAsync(string number, CancellationToken cancellationToken)
        {
            if (!ulong.TryParse(number, out ulong r) || number.Length != 16)
                throw new InvalidCardNumberException();
            return new CardModel(await _cardStore.GetCardByNumberAsync(number, cancellationToken));
        }
        public async Task<CardModel[]> GetUserCardsAsync(Guid userId, CancellationToken cancellationToken)
        {
            Card[] cardArray = await _cardStore.GetUserCardsAsync(userId, cancellationToken);
            CardModel[] cardModelArray = new CardModel[cardArray.Length];
            for (int i = 0; i<cardArray.Length; i++)
            {
                cardModelArray[i]=new CardModel(cardArray[i]);
            }
            return cardModelArray;
        }
        public async Task AddCardAsync(CardModel cardModel, Guid userId, CancellationToken cancellationToken)
        {
            await _cardStore.AddCardAsync(cardModel.ToCard(userId), cancellationToken);
        }
        public async Task RemoveCardAsync(string number, CancellationToken cancellationToken)
        {
            if (!ulong.TryParse(number, out ulong r)||number.Length!=16)
                throw new InvalidCardNumberException();
            await _cardStore.RemoveCardAsync(number, cancellationToken);
        }
    }
}

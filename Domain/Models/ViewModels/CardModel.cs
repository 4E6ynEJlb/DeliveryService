using Domain.Models.Entities.SQLEntities;
using System.Text.Json.Serialization;
using Domain.Models.ApplicationModels.Exceptions;

namespace Domain.Models.VievModels
{
    public class CardModel
    {
        public CardModel(Card card)
        {
            Number = card.Number;
            CVV = card.CVV;
            Valid = card.Valid;
            Holder = card.Holder;
        }
        [JsonConstructor]
        public CardModel(string number, short cVV, DateOnly valid, string holder)
        {
            Number = number;
            CVV = cVV;
            Valid = valid;
            Holder = holder;
        }
        public string Number { get; set; }
        public short CVV { get; set; }
        public DateOnly Valid { get; set; }
        public string Holder { get; set; }
        public Card ToCard(Guid userId)
        {
            if (!ulong.TryParse(Number, out var number) || Number.Length != 16)
                throw new InvalidCardNumberException();
            return new Card() { Number =  Number, CVV = CVV, Holder = Holder, Valid = Valid, UserId = userId };
        }
    }
}

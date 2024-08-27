namespace Domain.Models.Entities.SQLEntities
{
    public class Card
    {
        public required string Number { get; set; }
        public short CVV { get; set; }
        public DateOnly Valid { get; set; }
        public required string Holder { get; set; }
        public Guid UserId { get; set; }
        /// <summary>
        /// Field is null, do not use
        /// </summary>
#pragma warning disable CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Возможно, стоит объявить поле как допускающее значения NULL.
        public User User { get; set; }
#pragma warning restore CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Возможно, стоит объявить поле как допускающее значения NULL.
    }
}

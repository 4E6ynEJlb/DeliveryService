namespace Domain.Models.Entities.SQLEntities
{
    public class User
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public DateOnly? BirthDate { get; set; }
        public string? TelegramId { get; set; }
        public ICollection<Card> Cards { get; set; }
        public decimal Bonuses { get; set; }
        public required string Login { get; set; }
        public required string Password { get; set; }
        public bool IsAdmin { get; set; }
        public User()
        {
            Cards = new List<Card>();
        }
    }
}

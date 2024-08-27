namespace Domain.Models.Entities.SQLEntities
{
    public class Product
    {
        public Guid Id { get; set; }
        public int Article { get; set; }
        public decimal Price { get; set; }
        public required string Title { get; set; }
        public string? ImageName { get; set; }
        public TimeOnly? AverageCookingTime { get; set; }
        public int AlreadyCooked { get; set; }
        public float? Rating { get; set; }
        public int AlreadyRated { get; set; }
        public bool Visible { get; set; }
    }
}

namespace Domain.Models.Entities.SQLEntities
{
    public class Restaurant
    {
        public required string Adress { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public required string Login { get; set; }
        public required string Password { get; set; }
    }
}

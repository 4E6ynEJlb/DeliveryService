using Domain.Models.Entities.SQLEntities;

namespace Domain.Models.VievModels
{
    public class ProductInputModel
    {
        public int Article { get; set; }
        public decimal Price { get; set; }
        public required string Title { get; set; }
        public Product ToProduct()
        {
            return new Product() { Article = Article, Price = Price, AlreadyCooked = 0, AlreadyRated = 0, Visible = false, Title = Title, Id = Guid.NewGuid() };  
        }
    }
}

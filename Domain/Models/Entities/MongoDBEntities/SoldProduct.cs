using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Models.Entities.MongoDBEntities
{
    public class SoldProduct
    {
        public SoldProduct(int article)
        {
            Id = Guid.NewGuid();
            Article = article;
        }
        [BsonId]
        public Guid Id { get; set; }
        [BsonElement()]
        public DateTime ExpireAt { get; set; }
        [BsonElement]
        public int Article {  get; set; }
    }
}

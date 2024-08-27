using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Models.Entities.MongoDBEntities
{
    public class Report
    {
        [BsonId]
        public DateTime Received { get; set; }
        [BsonElement]
        public Guid UserId { get; set; }
        [BsonElement]
        public required string Message { get; set; }
    }
}

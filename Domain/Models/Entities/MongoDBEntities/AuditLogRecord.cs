using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Models.Entities.MongoDBEntities
{
    public class AuditLogRecord
    {
        public AuditLogRecord(Guid adminId, string message) 
        {
            AdminId = adminId;
            Message = message;
        }
        [BsonId]
        public DateTime Recorded { get; set; }
        [BsonElement()]
        public DateTime ExpireAt {  get; set; }
        [BsonElement]
        public Guid AdminId { get; set; }
        [BsonElement]
        public string Message { get; set; }
    }
}

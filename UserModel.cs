using MongoDB.Bson.Serialization.Attributes;

namespace rossgram
{
    public class UserModel
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public List<string> Images { get; set; }

    }
}

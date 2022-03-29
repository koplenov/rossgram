using MongoDB.Bson.Serialization.Attributes;

namespace rossgram
{
    public class UserModel
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public List<string> Images { get; set; }

        public UserModel() { }

        public UserModel(string login, string pass, string role)
        {
            this.Images = new List<string>();
            this.Login = login;
            this.Role = role;
            this.Password = pass;
        }

    }
}

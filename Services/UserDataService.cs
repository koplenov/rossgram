using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace rossgram
{
    public class UserDataService
    {
        private readonly IMongoCollection<UserModel> _users;

        public UserDataService()
        {
            var mongoClient = new MongoClient("mongodb://localhost:27017");
            _users = mongoClient.GetDatabase("exceed")
                .GetCollection<UserModel>("profiles");
        }
        public UserModel? Getuser(string login) => _users.Find(u => u.Login == login).FirstOrDefault();
        public void AddUser(UserModel user) => _users.InsertOne(user);
        public void DeleteUser(string login) => _users.DeleteOne(u=>u.Login== login);
        public void UpdateUser(string login, UserModel user) => _users.ReplaceOne(u => u.Login == login, user);
    }
}

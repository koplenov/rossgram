using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace rossgram
{
    public class UserDataService
    {
        private readonly IMongoCollection<UserModel> users;

        public UserDataService(IOptions<UserDataDBOptions> settings)
        {
            var MongoClient = new MongoClient(settings.Value.ConnectionString);
            users = MongoClient.GetDatabase(settings.Value.DatabaseName)
                .GetCollection<UserModel>(settings.Value.CollectionName);
        }
        public UserModel Getuser(string login) => users.Find(u => u.Login == login).FirstOrDefault();
        public void AddUser(UserModel user) => users.InsertOne(user);
        public void DeleteUser(string login) => users.DeleteOne(u=>u.Login== login);
        public void UpdateUser(string login, UserModel user) => users.ReplaceOne(u => u.Login == login, user);
    }
}

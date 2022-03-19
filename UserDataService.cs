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
        public UserModel Getuser(string name) => users.Find(u => u.Name == name).FirstOrDefault();
        public void AddUser(UserModel user) => users.InsertOne(user);
        public void DeleteUser(string name) => users.DeleteOne(name);
        public void UpdateUser(string name, UserModel user) => users.ReplaceOne(u => u.Name == name,user);
    }
}

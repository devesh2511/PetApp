using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetApp.Utility.DTOs;

namespace PetApp.Services.Repository
{
    public class UserServiceDB
    {
        private readonly IMongoCollection<UserDB> _userCollection;

        public UserServiceDB(
            IOptions<DatabaseSettings> UserDatabaseSettings)
        {
            var mongoClient = new MongoClient(UserDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(UserDatabaseSettings.Value.DatabaseName);

            _userCollection = mongoDatabase.GetCollection<UserDB>("Users");
        }

        public async Task<List<UserDB>> GetAsync() =>
            await _userCollection.Find(_ => true).ToListAsync();

        public async Task<UserDB> GetAsync(string username) =>
            await _userCollection.Find(x => x.Username == username).FirstOrDefaultAsync();

        public async Task CreateAsync(UserDB newUser) =>
            await _userCollection.InsertOneAsync(newUser);

        public async Task UpdateAsync(string username, UserDB updatedUser) =>
            await _userCollection.ReplaceOneAsync(x => x.Username == username, updatedUser);

        public async Task RemoveAsync(string username) =>
            await _userCollection.DeleteOneAsync(x => x.Username == username);
    }
}

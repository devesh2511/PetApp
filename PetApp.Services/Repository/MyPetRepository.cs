using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetApp.Utility.DTOs;
using PetApp.Utility.DTOs.Admin;

namespace PetApp.Services.Repository
{
    public class MyPetRepository
    {
        private readonly IMongoCollection<myPetDB> _myPetCollection;

        public MyPetRepository(
            IOptions<DatabaseSettings> UserDatabaseSettings)
        {
            var mongoClient = new MongoClient(UserDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(UserDatabaseSettings.Value.DatabaseName);

            _myPetCollection = mongoDatabase.GetCollection<myPetDB>("MyPets");
        }

        public async Task<List<myPetDB>> GetAllPets() =>
            await _myPetCollection.Find(_ => true).ToListAsync();

        public async Task<myPetDB> GetOnePet(string owner, string petName, int petId) =>
            await _myPetCollection.Find(x => x.owner == owner && x.petName == petName && x.petId == petId).FirstOrDefaultAsync();

        public async Task<List<myPetDB>> GetUserPets(string owner) =>
           await _myPetCollection.Find(x => x.owner == owner).ToListAsync();

        public async Task CreateAsync(myPetDB newPet) =>
            await _myPetCollection.InsertOneAsync(newPet);

        public async Task UpdateAsync(string petName, int age, int petId, myPetDB updatedUser) =>
            await _myPetCollection.ReplaceOneAsync(x => x.petName == petName && x.petId == petId, updatedUser);

        public async Task RemoveAsync(string petName, int age, int petId) =>
            await _myPetCollection.DeleteOneAsync(x => x.petName == petName && x.petId == petId);
    }
}

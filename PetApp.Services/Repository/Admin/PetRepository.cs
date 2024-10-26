using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetApp.Utility.DTOs;
using PetApp.Utility.DTOs.Admin;

namespace PetApp.Services.Repository.Admin
{
    public class PetRepository
    {
        private readonly IMongoCollection<PetsDB> _petsCollection;
        //private readonly IMongoCollection<Counter> _counterCollection;

        public PetRepository(
            IOptions<DatabaseSettings> UserDatabaseSettings)
        {
            var mongoClient = new MongoClient(UserDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(UserDatabaseSettings.Value.DatabaseName);

            _petsCollection = mongoDatabase.GetCollection<PetsDB>("Admin-Pets");
            //_counterCollection = mongoDatabase.GetCollection<Counter>("counters");
        }

        public async Task<List<PetsDB>> GetAllPets() =>
            await _petsCollection.Find(_ => true).ToListAsync();

        public async Task<PetsDB> GetOnePet(int petId) =>
            await _petsCollection.Find(x => x.petId == petId).FirstOrDefaultAsync();

        public async Task<List<PetsDB>> GetAllByPetClass(string petClass) =>
           await _petsCollection.Find(x => x.petClass == petClass).ToListAsync();

        public async Task<List<PetsDB>> GetAllByPetType(string petType) =>
          await _petsCollection.Find(x => x.petType == petType).ToListAsync();

        public async Task CreateAsync(PetsDB newPet) =>
            await _petsCollection.InsertOneAsync(newPet);

        //public async Task UpdateAsync(string petName, int age, PetType petType, PetsDB updatedUser) =>
        //    await _userCollection.ReplaceOneAsync(x => x.petName == petName && x.petType == petType.ToString(), updatedUser);

        //public async Task RemoveAsync(string petName, int age, PetType petType) =>
        //    await _userCollection.DeleteOneAsync(x => x.petName == petName && x.petType == petType.ToString());

        public async Task<int> GetNextSequenceValue()
        {
            var x = await _petsCollection.Find(_ => true).ToListAsync();
            PetsDB? petsDB = x.OrderByDescending(x => x.petId).FirstOrDefault();
            var y = petsDB.petId;

            var z = y + 1;

            return z;
        }
        public async Task AddNewPet(PetsDB pet)
        {
            // Get the next petId
            pet.petId =  await GetNextSequenceValue();

            await _petsCollection.InsertOneAsync(pet);
        }

        //public class Counter
        //{
        //    public string _id { get; set; }
        //    public int value { get; set; }
        //}

        //public async Task<int> GetNextSequenceValue()
        //{
        //    var filter = Builders<Counter>.Filter.Eq("_id", "petIdCounter");
        //    var update = Builders<Counter>.Update.Inc("value", 1);
        //    var result = await _counterCollection.FindOneAndUpdateAsync(filter, update, new FindOneAndUpdateOptions<Counter, Counter> { ReturnDocument = ReturnDocument.After });
        //    return result.value;
        //}

        //public async Task AddNewPet(PetsDB pet)
        //{
        //    pet.petId = await GetNextSequenceValue();
        //    await _petsCollection.InsertOneAsync(pet);
        //}

    }
}

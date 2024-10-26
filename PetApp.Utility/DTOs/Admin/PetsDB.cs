using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PetApp.Utility.DTOs.Admin
{
    public class PetsDB
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; internal set; }
        public int petId { get; set; }
        public string admin { get; set; }
        public string petClass { get; set; }
        public string? petType { get; set; }
        public string? petBreed { get; set; }
        public DateOnly createdOn { get; set; }
    }

    public class Sequence
    {
        [BsonId]
        public string Id { get; set; }  // Usually "petId" or similar
        public int SequenceValue { get; set; }
    }

    public class adminPetRequest
    {
        public PetClass petClass { get; set; }
        public PetType petType { get; set; }
        public string? petBreed { get; set; }
        public DateOnly createdOn { get; set; }
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum PetClass
    {
        Animal,
        Bird,
        Fish,
        Other
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum PetType
    {
        Camel,
        Cat,
        Dog,
        Donkey,
        Cattle,
        Horse,
        Pig,
        Goat,
        Other
    }

}

using MongoDB.Bson.Serialization.Attributes;
using PetApp.Utility.DTOs.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PetApp.Utility.DTOs
{
    public class petRequest
    {
        public string owner {  get; set; }
        public string petName { get; set; }
        public int petId { get; set; }
        public PetSex sex { get; set; }

        public DateOnly birthDate { get; set; }
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum PetSex
    {
        Male,
        Female,
        Other
    }

    public class myPetDB
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; internal set; }
        public string owner { get; set; }
        public string petName { get; set; }
        public DateOnly birthDate { get; set; }
        public int petId { get; set; }
        public string sex { get; set; }
        
    }

    public class myPetResponse
    {
        public string petName { get; set; }
        public DateOnly birthDate { get; set; }
        public int age { get; set; }
        public int petId { get; set; }
        public string sex { get; set; }
        public string petClass { get; set; }
        public string? petType { get; set; }
        public string? petBreed { get; set; }
    }

}

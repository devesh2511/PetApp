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
        public DateOnly birthDate { get; set; }
        public PetType petType { get; set; }
    }

    public class myPetDB
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; internal set; }
        public string owner { get; set; }
        public string petName { get; set; }
        public DateOnly birthDate { get; set; }
        public string petType { get; set; }

        public string petBreed { get; set; }
        
    }

    public class myPetResponse
    {
        public string petName { get; set; }
        public DateOnly birthDate { get; set; }
        public int age { get; set; }
        public string petType { get; set; }
    }

}

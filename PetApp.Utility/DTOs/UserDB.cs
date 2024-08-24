using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetApp.Utility.DTOs
{
    public class User
    {
        public string Username { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public long Phone { get; set; }
    }

    public class UserDB : User
    {
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; internal set; }
    }

    public class LogedUser : User
    {
        public string Token { get; set; }
    }

    public class UserRequest : User
    {
        public string Password { get; set; }
    }
}

using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatServer.Entity.EntityProperty;

namespace ChatServer.Entity.Meta.Profile
{
    public class ChatUserProfile
    {
        [BsonId]
        public Guid ID { get; set; }
        [BsonElement("Email")]
        public string Email { get; set; }
        [BsonElement("Password")]
        public string PassHashed { get; set; }
        [BsonElement("FirstName")]
        public string FirstName { get; set; }
        [BsonElement("LastName")]
        public string LastName { get; set; }
        [BsonElement("DoB")]
        public DateTime DoB { get; set; }
        [BsonElement("Gender")]
        public Gender Gender { get; set; }
        [BsonElement("LastLogoff")]
        public DateTime LastLogoff { get; set; }
        [BsonElement("Banned")]
        public bool Banned { get; set; } = false;
    }
}

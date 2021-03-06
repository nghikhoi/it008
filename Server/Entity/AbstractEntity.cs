using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer.Entity
{
    public abstract class AbstractEntity : IEntity
    {
        [BsonId]
        public Guid ID { get; set; }

        public abstract bool Save();
    }
}

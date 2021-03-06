using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatServer.Cache.Core;
using ChatServer.IO.Entity.Property;

namespace ChatServer.Entity.EntityProperty
{
    public class Relation
    {
        [BsonIgnore]
        static LRUCache<Guid, Relation> Cache = new LRUCache<Guid, Relation>(1000, 50);

        Guid source;
        Guid user1;
        Guid user2;

        [BsonId]
        public Guid ID { get; set; } = Guid.NewGuid();

        public Relation()
        {

        }

        public Relation(Guid user1, Guid user2, int source = 0)
        {
            this.User1 = user1;
            this.User2 = user2;
            if ((source & 1) == 1)
            {
                this.source = this.User1;
            } else
            {
                this.source = this.User2;
            }
        }

        [BsonElement("User1")]
        public Guid User1 
        { 
            get => user1;
            set {
                if (value.Equals(User2))
                {
                    throw new Exception("User1 must be distinguished from User2");
                }
                user1 = value;
            }
        }

        [BsonElement("User2")]
        public Guid User2
        {
            get => user2;
            set
            {
                if (value.Equals(User2))
                {
                    throw new Exception("User2 must be distinguished from User1");
                }
                user2 = value;
            }
        }

        [BsonElement("Type")]
        public RelationType RelationType { get; set; } = RelationType.None;

        [BsonElement("Source")]
        public Guid Source
        {
            get => source;
            set
            {
                if (!value.Equals(User1) &&  !value.Equals(User2))
                {
                    throw new EntryPointNotFoundException("Source user must be equal to user1 or user2");
                }
                this.source = value;
            }
        }

        [BsonElement("LastUnblock")]
        public DateTime LastUnblock { get; set; } = DateTime.MinValue;

        public void Save()
        {
            new RelationStore().Save(this);
        }

        public static Relation Get(Guid id, bool ignoreCache = false) {
            if (Cache.Contains(id) && !ignoreCache)
            {
                return Cache.Get(id);
            }

            Relation relation = new RelationStore().Load(id);

            if (relation != null && !ignoreCache)
            {
                Cache.AddReplace(id, relation);
            }

            return relation;
        }
    }
}

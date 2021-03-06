using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatServer.Entity.EntityProperty;
using ChatServer.IO.Storage;

namespace ChatServer.IO.Entity.Property
{
    public class RelationStore : Store
    {
        
        public Relation Load(Guid id)
        {
            return Mongo.Instance.Get<Relation>(Mongo.RelationCollectionName, collection =>
            {
                var condition = Builders<Relation>.Filter.Eq(p => p.ID, id);
                var result = collection.Find(condition).Limit(1).ToList();
                if (result.Count > 0)
                {
                    return result[0];
                }
                return null;
            });
        }

        public void Save(Relation relation)
        {
            lock (relation)
            {
                Mongo.Instance.Set<Relation>(Mongo.RelationCollectionName, collection =>
                {
                    var condition = Builders<Relation>.Filter.Eq(p => p.ID, relation.ID);
                    collection.ReplaceOneAsync(condition, relation, new UpdateOptions() { IsUpsert = true });
                });
            }
        }
    }
}

using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatServer.Entity.EntityProperty;
using ChatServer.IO.Storage;

namespace ChatServer.IO.Message
{
    public class AttachmentStore : Store
    {
        
        public static void Map(Guid hash, String fileName)
        {
            FileMap map = new FileMap(hash, fileName);
            Mongo.Instance.Set<FileMap>(Mongo.FileMapName, collection =>
            {
                var condition = Builders<FileMap>.Filter.Eq(p => p.Hash, hash);
                collection.ReplaceOne(condition, map, new UpdateOptions() { IsUpsert = true });
            });
        }

        public static String Parse(Guid hash)
        {
            FileMap map = Mongo.Instance.Get<FileMap>(Mongo.FileMapName, collection =>
            {
                var condition = Builders<FileMap>.Filter.Eq(p => p.Hash, hash);
                var result = collection.Find(condition).Limit(1).ToList();
                return result.FirstOrDefault();
            });

            if (map == null) return null;

            return map.FileName;
        }


    }

}

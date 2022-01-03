using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;

namespace ChatServer.Entity.EntityProperty {
    public class FileMap {
        [BsonId]
        public Guid Hash { get; set; }
        [BsonElement("Name")]
        public string FileName { get; set; }

        public FileMap() {

        }

        public FileMap(Guid hash, String fileName) {
            this.Hash = hash;
            this.FileName = fileName;
        }
    }
}

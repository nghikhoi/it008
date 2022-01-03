using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatServer.Entity.Notification;
using ChatServer.IO.Storage;
using MongoDB.Driver;

namespace ChatServer.IO.Notification {
    public class NotificationStore : Store {
        private readonly object _lock = new object();

        public AbstractNotification Load(Guid id) {
            return Mongo.Instance.Get<AbstractNotification>(Mongo.NotificationCollectionName, collection => {
                var condition = Builders<AbstractNotification>.Filter.Eq(p => p.Id, id);
                var result = collection.Find(condition).Limit(1).ToList();
                if (result.Count > 0) {
                    return result[0];
                }
                return null;
            });
        }

        public void Save(AbstractNotification notification) {
            lock (_lock) {
                Mongo.Instance.Set<AbstractNotification>(Mongo.NotificationCollectionName, collection => {
                    var condition = Builders<AbstractNotification>.Filter.Eq(p => p.Id, notification.Id);
                    collection.ReplaceOneAsync(condition, notification, new UpdateOptions() { IsUpsert = true });
                });
            }
        }
    }
}

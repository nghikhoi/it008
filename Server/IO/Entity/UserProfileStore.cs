using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatServer.Entity;
using ChatServer.IO.Storage;

namespace ChatServer.IO.Entity
{
    public class UserProfileStore : Store
    {

        private ProjectionDefinition<ChatUserProfile> ChatUserProfileProjection = Builders<ChatUserProfile>.Projection
            .Include(p => p.ID)
            .Include(p => p.Email)
            .Include(p => p.PassHashed)
            .Include(p => p.FirstName)
            .Include(p => p.LastName)
            .Include(p => p.DoB)
            .Include(p => p.Gender)
            .Include(p => p.LastLogoff)
            .Include(p => p.Banned);

        private ChatUserProfile LoadBy(FilterDefinition<ChatUserProfile> condition) {
            return Mongo.Instance.Get<ChatUserProfile>(Mongo.UserCollectionName, collection => {
                var result = collection.Find(condition).Project<ChatUserProfile>(ChatUserProfileProjection).Limit(1).ToList();
                if (result.Count > 0)
                {
                    return result[0];
                }
                return null;
            });
        }
        
        public ChatUserProfile LoadByEmail(string email) {
            return LoadBy(Builders<ChatUserProfile>.Filter.Eq(p => p.Email, email.ToLower()));
        }
        
        public ChatUserProfile LoadById(Guid id) {
            return LoadBy(Builders<ChatUserProfile>.Filter.Eq(p => p.ID, id));
        }

        public ChatUserProfile LoadById(string id)
        {
            return LoadById(Guid.Parse(id));
        }
    }
}

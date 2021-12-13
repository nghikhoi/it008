using System;
using ChatServer.Cache.Core;
using ChatServer.Entity.Conversation;
using ChatServer.IO.Message;

namespace ChatServer.Entity
{
    public static class ConversationManager
    {
        private static LRUCache<Guid, AbstractConversation> CachedConversation = new LRUCache<Guid, AbstractConversation>(1000, 10); 


        public static AbstractConversation GetConversation(Guid id)
        {
            CachedConversation.TryGet(id, out var result);

            if (result == null)
            {
                result = new ConversationStore().Load(id);
                CachedConversation.AddReplace(id, result);
            }

            return result;
        }
    }
}

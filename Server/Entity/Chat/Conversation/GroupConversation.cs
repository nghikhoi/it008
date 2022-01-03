using MongoDB.Bson.Serialization.Attributes;

namespace ChatServer.Entity
{
    public class GroupConversation : AbstractConversation
    {

        [BsonElement(nameof(ConversationName))]
        public string ConversationName { get; set; }

        [BsonElement(nameof(ConversationAvatar))]
        public string ConversationAvatar { get; set; }

    }
}

using System;
using ChatServer.Entity;
using ChatServer.Entity.EntityProperty;
using CNetwork;
using CNetwork.Sessions;
using CNetwork.Utils;
using DotNetty.Buffers;

namespace ChatServer.Network.Packets
{
    public class BuzzRequest : IPacket
    {
        public Guid ConversationID { get; set; }

        public void Decode(IByteBuffer buffer)
        {
            ConversationID = Guid.Parse(ByteBufUtils.ReadUTF8(buffer));
        }

        public IByteBuffer Encode(IByteBuffer byteBuf)
        {
            throw new NotImplementedException();
        }

        public void Handle(ISession session)
        {
            ChatSession chatSession = session as ChatSession;

            if (!chatSession.Owner.ConversationID.Contains(ConversationID)) return;

            AbstractConversation conversation = ConversationManager.GetConversation(ConversationID);

            if (conversation == null || conversation is GroupConversation) return;

            if (chatSession.Owner.LastBuzz.ContainsKey(ConversationID))
            {
                chatSession.Owner.LastBuzz.TryGetValue(ConversationID, out var lastBuzz);
                if (DateTime.Now.Ticks - lastBuzz < 10000000 * 10) return;
            }
            chatSession.Owner.LastBuzz.TryRemove(ConversationID, out var lastz);
            chatSession.Owner.LastBuzz.TryAdd(ConversationID, DateTime.Now.Ticks);

            BuzzResponse message = new BuzzResponse() {
                SenderID = chatSession.Owner.ID.ToString(),
                ConversationID = this.ConversationID.ToString()
            };

            foreach (Guid guid in conversation.Members)
            {
                if (ChatUserManager.IsOnline(guid) && guid != chatSession.Owner.ID)
                {
                    chatSession.Owner.Relationship.TryGetValue(guid, out var relationID);
                    Relation relation = Relation.Get(relationID);
                    if (relation == null || relation.RelationType != RelationType.Friend) continue;
                    ChatUserManager.LoadUser(guid).Send(message);
                } 
            }
        }
    }
}

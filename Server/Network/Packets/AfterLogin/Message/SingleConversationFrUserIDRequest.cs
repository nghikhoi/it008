using System;
using System.Collections.Generic;
using System.Linq;
using ChatServer.Entity;
using ChatServer.Entity.Conversation;
using ChatServer.IO.Message;
using CNetwork;
using CNetwork.Sessions;
using CNetwork.Utils;
using DotNetty.Buffers;

namespace ChatServer.Network.Packets
{
    public class SingleConversationFrUserIDRequest : AbstractRequestPacket
    {
        public Guid TargetID { get; set; } = Guid.Empty;

        public override void Decode(IByteBuffer buffer)
        {
            try
            {
                TargetID = Guid.Parse(ByteBufUtils.ReadUTF8(buffer));
            }
            catch
            {

            }
        }

        public override IPacket createResponde(ISession session) {
            if (TargetID.Equals(Guid.Empty)) return null;

            ChatUser targetUser = ChatUserManager.LoadUser(TargetID);
            if (targetUser == null) return null;

            ChatSession chatSession = session as ChatSession;

            ConversationStore store = new ConversationStore();

            Guid resultID = Guid.NewGuid();
            bool flag = true;

            var CommonConversation = targetUser.ConversationID.Intersect(chatSession.Owner.ConversationID);

            foreach (Guid conversationID in CommonConversation)
            {
                AbstractConversation conversation = store.Load(conversationID);
                if (conversation is SingleConversation)
                {
                    resultID = conversation.ID;
                    flag = false;
                    break;
                }
            }

            if (flag)
            {
                chatSession.Owner.ConversationID.Add(resultID); 
                targetUser.ConversationID.Add(resultID);

                store.Save(new SingleConversation()
                {
                    ID = resultID,
                    Members = new HashSet<Guid>() { chatSession.Owner.ID, targetUser.ID },
                    ConversationName = "~"
                });

                chatSession.Owner.Save();
                targetUser.Save();
            }

            SingleConversationFrUserIDResponse response = new SingleConversationFrUserIDResponse();
            response.UserID = TargetID;
            response.ResponseID = resultID;
            return response;
        }
    }
}

using CNetwork;
using CNetwork.Sessions;
using CNetwork.Utils;
using DotNetty.Buffers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatServer.Entity;
using ChatServer.Entity.EntityProperty;
using ChatServer.IO.Message;
using ChatServer.MessageCore.Conversation;
using ChatServer.MessageCore.Message;

namespace ChatServer.Network.Packets.AfterLogin.DataPreparing
{
    public class ShortProfileRequest : AbstractRequestPacket
    {
        public Guid TargetID { get; set; }

        public override void Decode(IByteBuffer buffer)
        {
            TargetID = Guid.Parse(ByteBufUtils.ReadUTF8(buffer));
        }

        public override IPacket createResponde(ISession session) {
                  ChatSession chatSession = session as ChatSession;

            ChatUser targetUser = ChatUserManager.LoadUser(TargetID);

            if (targetUser == null) return null;
            
            ShortProfileResponse response = new ShortProfileResponse();

            response.ID = targetUser.ID.ToString();
            response.FirstName = targetUser.FirstName;
            response.LastName = targetUser.LastName;
            response.IsOnline = targetUser.IsOnline();
            response.LastLogout = targetUser.LastLogoff;

            ConversationStore store = new ConversationStore();

            response.PreviewCode = -1;
            response.ConversationID = "~";

            var CommonConversation = targetUser.ConversationID.Intersect(chatSession.Owner.ConversationID);

            foreach (Guid id in CommonConversation)
            {
                AbstractConversation conversation = store.Load(id);
                if (conversation is SingleConversation)
                {
                    response.ConversationID = conversation.ID.ToString();

                    AbstractMessage message = 
                        conversation.MessagesID.Count > 0 ?
                        new MessageStore().Load(conversation.MessagesID.Last(), conversation.ID) :
                        null;
                    if (message == null) break;

                    if (!message.Showable(chatSession.Owner.ID))
                    {
                        response.PreviewCode = 0;
                        break;
                    }

                    response.PreviewCode = message.GetPreviewCode();

                    if (message.GetPreviewCode() == 4)
                    {
                        response.LastMess = (message as TextMessage).Message;
                    }

                    break;
                }
            }

            if (chatSession.Owner.Relationship.ContainsKey(targetUser.ID))
            {
                response.RelationshipType = (int) Relation.Get(chatSession.Owner.Relationship[targetUser.ID]).RelationType;
            } else
            {
                response.RelationshipType = (int) Relation.Type.None;
            }

            return response;
        }
    }
}

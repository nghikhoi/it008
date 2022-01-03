using System;
using System.Collections.Generic;
using System.Linq;
using ChatServer.Entity;
using ChatServer.Entity.EntityProperty;
using ChatServer.IO.Entity;
using ChatServer.IO.Message;
using CNetwork;
using CNetwork.Sessions;
using CNetwork.Utils;
using DotNetty.Buffers;

namespace ChatServer.Network.Packets
{
    public class UserSearchRequest : AbstractRequestPacket
    {
        public String Email { get; set; }

        public override void Decode(IByteBuffer buffer)
        {
            Email = ByteBufUtils.ReadUTF8(buffer).ToLower();
        }

        public override IPacket createResponde(ChatSession session) {
                     ChatSession chatSession = session as ChatSession;

            UserSearchResponse response = new UserSearchResponse();
            List<String> UserIDs = new ChatUserStore().SearchUserIDByEmail(Email, (session as ChatSession).Owner);

            ConversationStore store = new ConversationStore();

            foreach (var item in UserIDs)
            {
                Guid userId = Guid.Parse(item);
                ChatUser targetUser = ChatUserManager.LoadUser(userId);
                SearchResult result = new SearchResult();

                result.ID = targetUser.ID.ToString();
                result.FirstName = targetUser.FirstName;
                result.LastName = targetUser.LastName;
                result.IsOnline = targetUser.IsOnline();
                result.LastLogout = targetUser.LastLogoff;


                result.PreviewCode = -1;
                result.ConversationID = "~";

                var CommonConversation = targetUser.ConversationID.Intersect(chatSession.Owner.ConversationID);

                foreach (Guid id in CommonConversation)
                {
                    AbstractConversation conversation = store.Load(id);
                    if (conversation is SingleConversation)
                    {
                        result.ConversationID = conversation.ID.ToString();

                        AbstractMessage message =
                            conversation.MessagesID.Count > 0 ?
                            new MessageStore().Load(conversation.MessagesID.Last(), conversation.ID) :
                            null;
                        if (message == null) break;

                        if (!message.Showable(chatSession.Owner.ID))
                        {
                            result.PreviewCode = 0;
                            break;
                        }

                        result.PreviewCode = message.GetPreviewCode();

                        if (message.GetPreviewCode() == 4)
                        {
                            result.LastMessage = (message as TextMessage).Message;
                        }

                        break;
                    }
                }

                if (chatSession.Owner.Relationship.ContainsKey(targetUser.ID))
                {
                    result.Relationship = (int) Relation.Get(chatSession.Owner.Relationship[targetUser.ID]).RelationType;
                }
                else
                {
                    result.Relationship = (int) RelationType.None;
                }

                response.Results.Add(result);
            }

            return response;
        }
    }
}

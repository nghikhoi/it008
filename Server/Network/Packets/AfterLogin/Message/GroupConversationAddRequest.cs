using System;
using System.Collections.Generic;
using System.Linq;
using ChatServer.Entity;
using ChatServer.Entity.Conversation;
using ChatServer.IO.Message;
using ChatServer.Utils;
using CNetwork;
using CNetwork.Sessions;
using CNetwork.Utils;
using DotNetty.Buffers;

namespace ChatServer.Network.Packets
{
    public class GroupConversationAddRequest : AbstractRequestPacket
    {
        public HashSet<Guid> Members { get; set; } = new HashSet<Guid>();
        public string GroupName;
        public string ConversationId;

        public override void Decode(IByteBuffer buffer)
        {
            int count = buffer.ReadInt();
            for (int i = 0; i < count; i++)
                Members.Add(Guid.Parse(ByteBufUtils.ReadUTF8(buffer)));
            GroupName = ByteBufUtils.ReadUTF8(buffer);
            ConversationId = ByteBufUtils.ReadUTF8(buffer);
        }

        public override IPacket createResponde(ISession session) {
            List<ChatUser> users = Members.Select(ChatUserManager.LoadUser).ToList();
            ConversationStore store = new ConversationStore();

            Guid resultID = Guid.NewGuid();
            if (!FastCodeUtils.NotEmptyStrings(GroupName) || GroupName == "~")
            {
                GroupName = string.Join(", ", users.Select(user => user.LastName).ToArray());
            }

            GroupConversation conversation;
            if (ConversationId == "~")
            {
                conversation = new GroupConversation() {
                    ID = resultID,
                    Members = Members.ToHashSet(),
                    ConversationName = GroupName
                };
            }
            else
            {
                conversation = (GroupConversation) store.Load(Guid.Parse(ConversationId));
            }
            users.ForEach(user =>
            {
                conversation.Nicknames.Add(user.ID, user.FullName);
                user.ConversationID.Add(resultID);
                user.Save();
            });
            store.SaveSync(conversation);

            GroupConversationAddedResponse response = new GroupConversationAddedResponse {
                GroupId = resultID
            };
            foreach (var user in users.Where(user => !user.ID.Equals(((ChatSession) session).Owner.ID)))
            {
                user.Send(response);
            }
            return response;
        }
    }
}

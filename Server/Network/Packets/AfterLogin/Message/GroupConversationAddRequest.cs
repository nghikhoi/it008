using System;
using System.Collections.Generic;
using System.Linq;
using ChatServer.Entity;
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

        public override IPacket createResponde(ChatSession session) {
            List<ChatUser> users = Members.Select(ChatUserManager.LoadUser).ToList();
            ConversationStore store = new ConversationStore();

            Guid resultID = Guid.NewGuid();
            if (!FastCodeUtils.NotEmptyStrings(GroupName) || GroupName == "~")
            {
                GroupName = ""; //string.Join(", ", users.Select(user => user.LastName).ToArray());
            }

            GroupConversation conversation;
            bool createNew = false;
            if (ConversationId == "~")
            {
                createNew = true;
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

            Queue<AnnouncementMessage> msgs = new Queue<AnnouncementMessage>();
            users.ForEach(user =>
            {
                conversation.Nicknames.Add(user.ID, user.FullName);
                user.ConversationID.Add(resultID);
                user.Save();
                if (!createNew) {
                    AnnouncementMessage msg = new AnnouncementMessage() {
                        Type = AnnouncementType.ADD_MEMBER,
                        Value = user.FullName
                    };
                    msgs.Enqueue(msg);
                }
            });
            store.SaveSync(conversation);

            GroupConversationAddedResponse response = new GroupConversationAddedResponse {
                GroupId = resultID
            };
            foreach (var user in users.Where(user => !user.ID.Equals(((ChatSession) session).Owner.ID)))
            {
                user.Send(response);
            }

            while (msgs.Count > 0)
            {
                AnnouncementMessage msg = msgs.Dequeue();
                conversation.SendMessage(msg, session, false);
            }

            conversation.SendIfOnline(new GetConversationShortInfoNotify() {
                ConversationId = conversation.ID
            });

            return response;
        }
    }
}

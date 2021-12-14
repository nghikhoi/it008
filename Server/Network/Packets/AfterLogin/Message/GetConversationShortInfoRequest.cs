using System;
using System.Linq;
using ChatServer.Entity.Conversation;
using ChatServer.IO.Entity;
using ChatServer.IO.Message;
using CNetwork;
using CNetwork.Sessions;
using CNetwork.Utils;
using DotNetty.Buffers;

namespace ChatServer.Network.Packets
{
    public class GetConversationShortInfoRequest : AbstractRequestPacket
    {
        public Guid ConversationID { get; set; }

        public override void Decode(IByteBuffer buffer)
        {
            ConversationID = Guid.Parse(ByteBufUtils.ReadUTF8(buffer));
        }

        public override IPacket createResponde(ISession session) {
            GetConversationShortInfoResponse packet = new GetConversationShortInfoResponse();

            packet.ConversationID = ConversationID.ToString();

            ChatSession chatSession = session as ChatSession;
            AbstractConversation conversation = new ConversationStore().Load(ConversationID);

            int cnt = 0;
            packet.ConversationName = "";
            if (string.IsNullOrEmpty(conversation.ConversationName) || conversation.ConversationName.Equals("~"))
            {
                foreach (var member in conversation.Members)
                {
                    if (member.CompareTo(chatSession.Owner.ID) == 0) continue;
                    string name = new ChatUserStore().Load(member).FirstName;
                    packet.ConversationName += name + ", ";
                    cnt++;

                    if (cnt >= 2) break;
                }

                if (cnt >= 2)
                    packet.ConversationName += "and " + (conversation.Members.Count - 3) + "others...";
                else
                    packet.ConversationName = packet.ConversationName.Replace(", ", "");
            }

            Guid avatar = conversation.Members.FirstOrDefault(s => s.CompareTo(chatSession.Owner.ID) != 0);
            packet.ConversationAvatar = avatar == null ? "~" : avatar.ToString();
            conversation.UpdateLastActive(chatSession);
            packet.LastActive = conversation.LastActive;
            return packet;
        }
    }
}

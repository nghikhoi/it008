using CNetwork;
using CNetwork.Sessions;
using CNetwork.Utils;
using DotNetty.Buffers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatServer.IO.Message;
using ChatServer.MessageCore.Conversation;
using Org.BouncyCastle.Ocsp;

namespace ChatServer.Network.Packets.AfterLogin.Message
{
    public class ConversationFrIDRequest : AbstractRequestPacket
    {
        public Guid ConversationID { get; set; }

        public override void Decode(IByteBuffer buffer)
        {
            ConversationID = Guid.Parse(ByteBufUtils.ReadUTF8(buffer));
        }

        public override IPacket createResponde(ISession session) {
            ChatSession chatSession = session as ChatSession;

            ConversationFrIDResponse packet = new ConversationFrIDResponse();
            packet.ConversationID = ConversationID.ToString();
            var conversationStore = new ConversationStore().Load(ConversationID);

            packet.LastActive = 0;
            packet.ConversationName = "";

            if (conversationStore == null)
            {
                packet.StatusCode = 404;
            }
            else
            {
                packet.StatusCode = 200;
                conversationStore.UpdateLastActive(chatSession);
                packet.LastActive = conversationStore.LastActive;

                if (conversationStore is SingleConversation)
                    packet.ConversationName = "~";
                else
                    packet.ConversationName = conversationStore.ConversationName;

                foreach (var member in conversationStore.Members)
                {
                    packet.Members.Add(member.ToString());
                }

                packet.LastMessID = conversationStore.MessagesID.Count - 1;
                packet.LastMediaID = conversationStore.MediaID.Count - 1;
                packet.LastAttachmentID = conversationStore.AttachmentID.Count - 1;
            }

            packet.BubbleColor = conversationStore.Color;

            // Update later
            packet.PreviewCode = -1;
            packet.PreviewContent = "";
            return packet;
        }
    }
}

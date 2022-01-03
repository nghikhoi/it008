using System;
using ChatServer.Entity;
using ChatServer.IO.Message;
using CNetwork;
using CNetwork.Sessions;
using CNetwork.Utils;
using DotNetty.Buffers;

namespace ChatServer.Network.Packets
{
    public class MessageFromConversationRequest : AbstractRequestPacket
    {
        public Guid ConversationID { get; set; }
        public int MessagePosition { get; set; }
        public int Quantity { get; set; }
        public bool LoadConversation { get; set; }

        public override void Decode(IByteBuffer buffer)
        {
            ConversationID = Guid.Parse(ByteBufUtils.ReadUTF8(buffer));
            MessagePosition = buffer.ReadInt();
            Quantity = buffer.ReadInt();
            LoadConversation = buffer.ReadBoolean();
        }

        public override IPacket createResponde(ChatSession session) {
            if (MessagePosition == -1) return null;

            ChatSession chatSession = session as ChatSession;
            ConversationStore store = new ConversationStore();

            AbstractConversation conversation = store.Load(ConversationID);

            MessageFromConversationResponse packet = new MessageFromConversationResponse();

            var messages = conversation.MessagesID;

            if (messages.Count == 0) return packet;
            MessageStore messageStore = new MessageStore();

            for (int i = MessagePosition; i >= Math.Max(0, MessagePosition - Quantity + 1); --i)
            {
                AbstractMessage mess = new MessageStore().Load(messages[i], ConversationID);

                if (mess != null)
                {
                    packet.SenderID.Add(mess.Author.ToString());
                    packet.Content.Add(mess);
                }
            }

            packet.LoadConversation = LoadConversation;
            return packet;
        }
    }
}

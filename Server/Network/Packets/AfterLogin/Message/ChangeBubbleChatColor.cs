using System;
using ChatServer.Entity;
using ChatServer.Entity.Conversation;
using CNetwork;
using CNetwork.Sessions;
using CNetwork.Utils;
using DotNetty.Buffers;

namespace ChatServer.Network.Packets
{
    public class ChangeBubbleChatColor : IPacket
    {
        public String ConversationID { get; set; }
        public int Color { get; set; }

        public void Decode(IByteBuffer buffer)
        {
            ConversationID = ByteBufUtils.ReadUTF8(buffer);
            Color = buffer.ReadInt();
        }

        public IByteBuffer Encode(IByteBuffer byteBuf)
        {
            ByteBufUtils.WriteUTF8(byteBuf, ConversationID);
            byteBuf.WriteInt(Color);

            return byteBuf;
        }

        public void Handle(ISession session)
        {
            Guid converID = Guid.Parse(ConversationID);
            AbstractConversation conversation = ConversationManager.GetConversation(converID);
            if (conversation == null) return;
            conversation.UpdateColor(Color, (session as ChatSession).Owner);
        }
    }
}

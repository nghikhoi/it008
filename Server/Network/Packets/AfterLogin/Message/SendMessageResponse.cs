using ChatServer.Entity.Message;
using CNetwork;
using CNetwork.Sessions;
using CNetwork.Utils;
using DotNetty.Buffers;

namespace ChatServer.Network.Packets
{
    public class SendMessageResponse : IPacket
    {
        public string ConversationID { get; set; }
        public string SenderID { get; set; }
        public AbstractMessage Message { get; set; }

        public void Decode(IByteBuffer buffer)
        {
        }

        public IByteBuffer Encode(IByteBuffer byteBuf)
        {
            ByteBufUtils.WriteUTF8(byteBuf, ConversationID);
            ByteBufUtils.WriteUTF8(byteBuf, SenderID);
            byteBuf = Message.EncodeToBuffer(byteBuf);

            return byteBuf;
        }

        public void Handle(ISession session)
        {
        }
    }
}

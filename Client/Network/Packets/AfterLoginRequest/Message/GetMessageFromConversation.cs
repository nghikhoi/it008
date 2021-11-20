using CNetwork;
using CNetwork.Sessions;
using CNetwork.Utils;
using DotNetty.Buffers;

namespace UI.Network.Packets.AfterLoginRequest.Message
{
    public class GetMessageFromConversation : RequestPacket
    {
        public string ConversationID { get; set; }
        public int MessagePosition { get; set; }
        public int Quantity { get; set; }
        public bool LoadConversation { get; set; } = false;

        public void Decode(IByteBuffer buffer)
        {
        }

        public IByteBuffer Encode(IByteBuffer byteBuf)
        {
            ByteBufUtils.WriteUTF8(byteBuf, ConversationID);
            byteBuf.WriteInt(MessagePosition);
            byteBuf.WriteInt(Quantity);
            byteBuf.WriteBoolean(LoadConversation);

            return byteBuf;
        }

        public void Handle(ISession session)
        {
        }
    }
}

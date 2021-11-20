using CNetwork;
using CNetwork.Sessions;
using CNetwork.Utils;
using DotNetty.Buffers;

namespace UI.Network.Packets.AfterLoginRequest.Message
{
    public class GetConversationShortInfo : RequestPacket
    {
        public string ConversationID { get; set; }

        public void Decode(IByteBuffer buffer)
        {
        }

        public IByteBuffer Encode(IByteBuffer byteBuf)
        {
            ByteBufUtils.WriteUTF8(byteBuf, ConversationID);
            return byteBuf;
        }

        public void Handle(ISession session)
        {
        }
    }
}

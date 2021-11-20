using CNetwork;
using CNetwork.Sessions;
using CNetwork.Utils;
using DotNetty.Buffers;

namespace UI.Network.Packets.AfterLoginRequest.Message
{
    public class SingleConversationFrUserID : RequestPacket
    {
        public string UserID { get; set; }

        public void Decode(IByteBuffer buffer)
        {
            // throw new NotImplementedException();
        }

        public IByteBuffer Encode(IByteBuffer byteBuf)
        {
            ByteBufUtils.WriteUTF8(byteBuf, UserID);
            return byteBuf;
        }

        public void Handle(ISession session)
        {
            // throw new NotImplementedException();
        }
    }
}

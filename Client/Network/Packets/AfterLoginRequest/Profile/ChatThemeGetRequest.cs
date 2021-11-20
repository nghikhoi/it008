using CNetwork;
using CNetwork.Sessions;
using DotNetty.Buffers;

namespace UI.Network.Packets.AfterLoginRequest.Profile
{
    public class ChatThemeGetRequest : RequestPacket
    {
        public void Decode(IByteBuffer buffer)
        {

        }

        public IByteBuffer Encode(IByteBuffer byteBuf)
        {
            return byteBuf;
        }

        public void Handle(ISession session)
        {

        }
    }
}

using CNetwork;
using CNetwork.Sessions;
using DotNetty.Buffers;

namespace UI.Network.Packets.AfterLoginRequest.Message
{
    public class RecentConversations : IPacket
    {
        public void Decode(IByteBuffer buffer)
        {
            // throw new NotImplementedException();
        }

        public IByteBuffer Encode(IByteBuffer byteBuf)
        {
            //throw new NotImplementedException();
            return byteBuf;
        }

        public void Handle(ISession session)
        {
            // throw new NotImplementedException();
        }
    }
}

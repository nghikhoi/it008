using CNetwork;
using CNetwork.Sessions;
using DotNetty.Buffers;

namespace ChatServer.Network.Packets
{
    public class GetSelfIDRequest : AbstractRequestPacket
    {
        public override void Decode(IByteBuffer buffer)
        {
            
        }

        public override IPacket createResponde(ChatSession session) {
            ChatSession chatSession = session as ChatSession;
            GetSelfIDResponse packet = new GetSelfIDResponse();
            packet.ID = chatSession.Owner.ID.ToString();
            return packet;
        }
    }
}

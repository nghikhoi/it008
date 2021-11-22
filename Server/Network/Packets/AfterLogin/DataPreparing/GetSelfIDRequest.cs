using CNetwork;
using CNetwork.Sessions;
using DotNetty.Buffers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer.Network.Packets.AfterLogin.DataPreparing
{
    public class GetSelfIDRequest : AbstractRequestPacket
    {
        public override void Decode(IByteBuffer buffer)
        {
            
        }

        public override IPacket createResponde(ISession session) {
            ChatSession chatSession = session as ChatSession;
            GetSelfIDResponse packet = new GetSelfIDResponse();
            packet.ID = chatSession.Owner.ID.ToString();
            return packet;
        }
    }
}

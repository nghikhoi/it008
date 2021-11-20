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
    public class GetSelfIDRequest : RequestPacket
    {
        public void Decode(IByteBuffer buffer)
        {
        }

        public IByteBuffer Encode(IByteBuffer byteBuf)
        {
            throw new NotImplementedException();
        }

        public void Handle(ISession session)
        {
            ChatSession chatSession = session as ChatSession;
            chatSession.Send(createResponde(session));
        }

        public IPacket createResponde(ISession session) {
            ChatSession chatSession = session as ChatSession;
            GetSelfIDResponse packet = new GetSelfIDResponse();
            packet.ID = chatSession.Owner.ID.ToString();
            return packet;
        }
    }
}

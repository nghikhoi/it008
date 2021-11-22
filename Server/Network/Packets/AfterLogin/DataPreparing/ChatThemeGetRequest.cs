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
    public class ChatThemeGetRequest : AbstractRequestPacket
    {
        
        public override void Decode(IByteBuffer buffer) {

        }

        public override IPacket createResponde(ISession session) {
            ChatSession chatSession = session as ChatSession;
            ChatThemeSetRequest response = new ChatThemeSetRequest();

            response.Theme = chatSession.Owner.ChatThemeSettings;

            session.Send(response);
            return response;
        }
        
    }
}

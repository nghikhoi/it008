using CNetwork;
using CNetwork.Sessions;
using DotNetty.Buffers;

namespace ChatServer.Network.Packets
{
    public class ChatThemeGetRequest : AbstractRequestPacket
    {
        
        public override void Decode(IByteBuffer buffer) {

        }

        public override IPacket createResponde(ChatSession session) {
            ChatSession chatSession = session as ChatSession;
            ChatThemeSetRequest response = new ChatThemeSetRequest();

            response.Theme = chatSession.Owner.ChatThemeSettings;

            session.Send(response);
            return response;
        }
        
    }
}

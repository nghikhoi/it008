using CNetwork;
using CNetwork.Sessions;
using DotNetty.Buffers;

namespace ChatServer.Network.Packets
{
    public class GetNearestSickerRequest : AbstractRequestPacket
    {
        public override void Decode(IByteBuffer buffer)
        {

        }

        public override IPacket createResponde(ISession session) {
            ChatSession chatSession = session as ChatSession;
            GetNearestSickerResponse response = new GetNearestSickerResponse();
            for (int i = 0; i < 20 && i < chatSession.Owner.NearestStickers.Count; i++)
            {
                response.NearestSticker.Add(chatSession.Owner.NearestStickers[i]);
            }

            return response;
        }
    }
}

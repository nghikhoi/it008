using CNetwork;
using CNetwork.Sessions;
using DotNetty.Buffers;

namespace ChatServer.Network.Packets
{
    public class GetBoughtStickerPacksRequest : AbstractRequestPacket
    {
        public override void Decode(IByteBuffer buffer)
        {

        }

        public override IPacket createResponde(ISession session) {
            ChatSession chatSession = session as ChatSession;
            GetBoughtStickerPacksResponse response = new GetBoughtStickerPacksResponse();
            response.BoughtStickerPacks.AddRange(chatSession.Owner.BoughtStickerPacks);
            return response;
        }
    }
}

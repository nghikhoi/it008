using ChatServer.Entity.Sticker;
using CNetwork;
using CNetwork.Sessions;
using DotNetty.Buffers;

namespace ChatServer.Network.Packets
{
    public class BuyStickerCategoryMixed : IPacket
    {
        public int CateID { get; set; }

        public void Decode(IByteBuffer buffer)
        {
            CateID = buffer.ReadInt();
        }

        public IByteBuffer Encode(IByteBuffer byteBuf)
        {
            byteBuf.WriteInt(CateID);

            return byteBuf;
        }

        public void Handle(ISession session)
        {
            ChatSession chatSession = session as ChatSession;

            if (!Sticker.LoadedCategories.ContainsKey(CateID)) return;
            if (chatSession.Owner.BoughtStickerPacks.Contains(CateID)) return;

            chatSession.Owner.BoughtStickerPacks.Add(CateID);
            chatSession.Owner.Save();

            chatSession.Owner.Send(this);
        }
    }
}

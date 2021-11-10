using System;
using CNetwork;
using CNetwork.Sessions;
using DotNetty.Buffers;

namespace UI.Network.Packets.AfterLoginRequest.Sticker
{
    public class BuyStickerCategoryRequest : IPacket
    {
        public int CateID { get; set; }

        public void Decode(IByteBuffer buffer)
        {
            throw new NotImplementedException();
        }

        public IByteBuffer Encode(IByteBuffer byteBuf)
        {
            byteBuf.WriteInt(CateID);

            return byteBuf;
        }

        public void Handle(ISession session)
        {
            throw new NotImplementedException();
        }
    }
}

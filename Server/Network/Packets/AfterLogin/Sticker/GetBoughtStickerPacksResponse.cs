using System;
using System.Collections.Generic;
using CNetwork;
using CNetwork.Sessions;
using DotNetty.Buffers;

namespace ChatServer.Network.Packets
{
    public class GetBoughtStickerPacksResponse : IPacket
    {
        public List<int> BoughtStickerPacks { get; set; } = new List<int>();

        public void Decode(IByteBuffer buffer)
        {
            throw new NotImplementedException();
        }

        public IByteBuffer Encode(IByteBuffer byteBuf)
        {
            foreach (int item in BoughtStickerPacks)
            {
                byteBuf.WriteInt(item);
            }
            byteBuf.WriteInt(-1402);

            return byteBuf;
        }

        public void Handle(ISession session)
        {
            throw new NotImplementedException();
        }
    }
}

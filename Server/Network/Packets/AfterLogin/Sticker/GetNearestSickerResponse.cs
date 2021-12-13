using System;
using System.Collections.Generic;
using CNetwork;
using CNetwork.Sessions;
using DotNetty.Buffers;

namespace ChatServer.Network.Packets
{
    public class GetNearestSickerResponse : IPacket
    {
        public List<int> NearestSticker { get; set; } = new List<int>();

        public void Decode(IByteBuffer buffer)
        {
            throw new NotImplementedException();
        }

        public IByteBuffer Encode(IByteBuffer byteBuf)
        {
            foreach (var stickerID in NearestSticker)
            {
                byteBuf.WriteInt(stickerID);
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

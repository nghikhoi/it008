using System;
using CNetwork;
using CNetwork.Sessions;
using CNetwork.Utils;
using DotNetty.Buffers;

namespace UI.Network.Packets
{
    public class TestPacket : IPacket
    {
        public String mess { get; set; }

        public void Decode(IByteBuffer buffer)
        {
            mess = ByteBufUtils.ReadUTF8(buffer);
        }

        public IByteBuffer Encode(IByteBuffer byteBuf)
        {
            ByteBufUtils.WriteUTF8(byteBuf, mess);
            return byteBuf;
        }

        public void Handle(ISession session)
        {
            Console.WriteLine("Received: " + mess);
        }
    }
}

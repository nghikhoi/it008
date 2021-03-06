using System;
using CNetwork;
using CNetwork.Sessions;
using CNetwork.Utils;
using DotNetty.Buffers;

namespace ChatServer.Network.Packets
{
    public class DisplayedProfileResponse : IPacket
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string DoB { get; set; }
        public string Town { get; set; }

        public void Decode(IByteBuffer buffer)
        {
            throw new NotImplementedException();
        }

        public IByteBuffer Encode(IByteBuffer byteBuf)
        {
            ByteBufUtils.WriteUTF8(byteBuf, ID);
            ByteBufUtils.WriteUTF8(byteBuf, Name);
            ByteBufUtils.WriteUTF8(byteBuf, Email);
            ByteBufUtils.WriteUTF8(byteBuf, DoB);
            ByteBufUtils.WriteUTF8(byteBuf, Town);
            return byteBuf;
        }

        public void Handle(ISession session)
        {
            throw new NotImplementedException();
        }
    }
}

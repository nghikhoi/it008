using System;
using ChatServer.Entity.EntityProperty;
using CNetwork;
using CNetwork.Sessions;
using CNetwork.Utils;
using DotNetty.Buffers;

namespace ChatServer.Network.Packets
{
    public class GetSelfProfileResponse : IPacket
    {
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String Town { get; set; } = "Default";
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }

        public void Decode(IByteBuffer buffer)
        {
            throw new NotImplementedException();
        }

        public IByteBuffer Encode(IByteBuffer byteBuf)
        {
            ByteBufUtils.WriteUTF8(byteBuf, FirstName);
            ByteBufUtils.WriteUTF8(byteBuf, LastName);
            ByteBufUtils.WriteUTF8(byteBuf, Town);
            ByteBufUtils.WriteUTF8(byteBuf, Email);
            byteBuf.WriteLong(DateOfBirth.Ticks);
            byteBuf.WriteInt((int)Gender);
            return byteBuf;
        }

        public void Handle(ISession session)
        {
            throw new NotImplementedException();
        }
    }
}

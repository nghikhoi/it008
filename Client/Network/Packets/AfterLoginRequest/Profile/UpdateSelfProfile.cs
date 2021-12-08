using System;
using CNetwork;
using CNetwork.Sessions;
using CNetwork.Utils;
using DotNetty.Buffers;
using UI.Models;

namespace UI.Network.Packets.AfterLoginRequest.Profile
{
    public class UpdateSelfProfile : IPacket
    {
        public UserProfile UserProfile { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String Town { get; set; } = "Default";
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }

        public void Decode(IByteBuffer buffer)
        {
            throw new NotImplementedException();
        }

        public IByteBuffer Encode(IByteBuffer byteBuf)
        {
            ByteBufUtils.WriteUTF8(byteBuf, UserProfile.FirstName);
            ByteBufUtils.WriteUTF8(byteBuf, UserProfile.LastName);
            ByteBufUtils.WriteUTF8(byteBuf, UserProfile.Town);
            ByteBufUtils.WriteUTF8(byteBuf, UserProfile.BirthDay.ToString());
            byteBuf.WriteInt((int) UserProfile.Gender);
            return byteBuf;
        }

        public void Handle(ISession session)
        {
            throw new NotImplementedException();
        }
    }
}

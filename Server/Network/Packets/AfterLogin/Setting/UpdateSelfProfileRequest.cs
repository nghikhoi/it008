using System;
using ChatServer.Entity;
using ChatServer.Entity.EntityProperty;
using CNetwork;
using CNetwork.Sessions;
using CNetwork.Utils;
using DotNetty.Buffers;

namespace ChatServer.Network.Packets
{
    public class UpdateSelfProfileRequest : AbstractRequestPacket
    {
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String Town { get; set; } = "Default";
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }

        public override void Decode(IByteBuffer buffer)
        {
            FirstName = ByteBufUtils.ReadUTF8(buffer);
            LastName = ByteBufUtils.ReadUTF8(buffer);
            Town = ByteBufUtils.ReadUTF8(buffer);
            DateOfBirth = DateTime.Parse(ByteBufUtils.ReadUTF8(buffer));
            Gender = (Gender)buffer.ReadInt();
        }

        public override IPacket createResponde(ISession session) {
            ChatSession chatSession = session as ChatSession;
            
            ChatUser user = chatSession.Owner;
            user.FirstName = FirstName;
            user.LastName = LastName;
            user.Town = Town;
            user.DateOfBirth = DateOfBirth;
            user.Gender = Gender;
            user.Save();

            GetSelfProfileResponse packet = new GetSelfProfileResponse();
            packet.FirstName = user.FirstName;
            packet.LastName = user.LastName;
            packet.Email = user.Email;
            packet.Town = user.Town;
            packet.DateOfBirth = user.DateOfBirth;
            packet.Gender = user.Gender;
            return packet;
        }
    }
}

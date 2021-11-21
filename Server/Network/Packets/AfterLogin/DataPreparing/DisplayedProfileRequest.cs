using CNetwork;
using CNetwork.Sessions;
using CNetwork.Utils;
using DotNetty.Buffers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatServer.Entity;

namespace ChatServer.Network.Packets.AfterLogin.DataPreparing
{
    public class DisplayedProfileRequest : AbstractRequestPacket
    {
        public Guid TargetID { get; set; }

        public override void Decode(IByteBuffer buffer)
        {
            TargetID = Guid.Parse(ByteBufUtils.ReadUTF8(buffer));
        }

        public override IPacket createResponde(ISession session) {
            ChatUser targetUser = ChatUserManager.LoadUser(TargetID);

            if (targetUser == null) return null;

            DisplayedProfileResponse packet = new DisplayedProfileResponse();
            packet.ID = targetUser.ID.ToString();
            packet.Name = targetUser.FirstName + " " + targetUser.LastName;
            packet.Email = targetUser.Email;
            packet.DoB = targetUser.DateOfBirth.ToString();
            packet.Town = targetUser.Town;

            return packet;
        }
    }
}

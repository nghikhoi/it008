using CNetwork;
using CNetwork.Sessions;
using DotNetty.Buffers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatServer.Entity;
using ChatServer.IO.Entity;

namespace ChatServer.Network.Packets.AfterLogin.DataPreparing
{
    public class GetSelfProfileRequest : AbstractRequestPacket
    {
        public override void Decode(IByteBuffer buffer)
        {
        }

        public override IPacket createResponde(ISession session) {
            ChatSession chatSession = session as ChatSession;

            ChatUser user = new ChatUserStore().Load(chatSession.Owner.ID);
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

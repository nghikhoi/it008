using ChatServer.Entity;
using ChatServer.IO.Entity;
using CNetwork;
using CNetwork.Sessions;
using DotNetty.Buffers;

namespace ChatServer.Network.Packets
{
    public class GetSelfProfileRequest : AbstractRequestPacket
    {
        public override void Decode(IByteBuffer buffer)
        {
        }

        public override IPacket createResponde(ChatSession session) {
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

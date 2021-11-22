using System;
using ChatServer.Entity;
using CNetwork;
using CNetwork.Sessions;
using DotNetty.Buffers;

namespace ChatServer.Network.Packets.AfterLogin.Notification
{
    public class GetNotificationsRequest : AbstractRequestPacket
    {
        public override void Decode(IByteBuffer buffer)
        {
        }

        public override IPacket createResponde(ISession session) {
            ChatSession chatSession = session as ChatSession;

            GetNotificationsResponse packet = new GetNotificationsResponse();
            ChatUser user = ChatUserManager.LoadUser(chatSession.Owner.ID);

            foreach (var noti in user.Notifications)
            {
                packet.Notifications.Add(noti);
            }

            return packet;
        }
    }
}

using ChatServer.Entity;
using ChatServer.Entity.Notification;
using ChatServer.IO.Notification;
using CNetwork;
using CNetwork.Sessions;
using DotNetty.Buffers;

namespace ChatServer.Network.Packets
{
    public class GetNotificationsRequest : AbstractRequestPacket
    {
        public override void Decode(IByteBuffer buffer)
        {
        }

        public override IPacket createResponde(ChatSession session) {
            ChatSession chatSession = session as ChatSession;

            GetNotificationsResponse packet = new GetNotificationsResponse();
            ChatUser user = ChatUserManager.LoadUser(chatSession.Owner.ID);
            NotificationStore store = new NotificationStore();

            foreach (var noti in user.Notifications)
            {
                AbstractNotification notification = store.Load(noti);
                if (notification == null) continue;
                packet.Notifications.Add(notification);
            }

            return packet;
        }
    }
}

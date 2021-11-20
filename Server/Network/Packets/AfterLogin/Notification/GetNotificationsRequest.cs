using System;
using ChatServer.Entity;
using CNetwork;
using CNetwork.Sessions;
using DotNetty.Buffers;

namespace ChatServer.Network.Packets.AfterLogin.Notification
{
    public class GetNotificationsRequest : RequestPacket
    {
        public void Decode(IByteBuffer buffer)
        {
        }

        public IByteBuffer Encode(IByteBuffer byteBuf)
        {
            throw new NotImplementedException();
        }

        public void Handle(ISession session)
        {
            session.Send(createResponde(session));
        }

        public IPacket createResponde(ISession session) {
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

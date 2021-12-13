using System;
using System.Collections.Generic;
using CNetwork;
using CNetwork.Sessions;
using CNetwork.Utils;
using DotNetty.Buffers;
using UI.Models.Notification;

namespace UI.Network.Packets.AfterLoginRequest.Notification
{
    public class GetNotificationsResult : IPacket
    {
        public List<AbstractNotification> Notifications = new List<AbstractNotification>();

        public void Decode(IByteBuffer buffer)
        {
            int count = buffer.ReadInt();
            for (int i = 0; i < count; i++)
            {
                NotificationType type = (NotificationType) buffer.ReadByte();
                AbstractNotification noti;
                switch (type)
                {
                    case NotificationType.FRIEND_REQUEST:
                    {
                        noti = new FriendRequestNotification();
                        break;
                    }
                    case NotificationType.ACCEPT_FRIEND_RESPONDE:
                    {
                        noti = new AcceptFriendNotification();
                        break;
                    }
                    case NotificationType.ANNOUNCEMENT:
                    {
                        continue;
                    }
                    default: continue;
                }
                noti.DecodeFromBuffer(buffer);
                Notifications.Add(noti);
            }
        }

        public IByteBuffer Encode(IByteBuffer byteBuf)
        {
            throw new NotImplementedException();
        }

        public void Handle(ISession session)
        {
        }
    }
}

using System;
using System.Collections.Generic;
using ChatServer.Entity.Notification;
using CNetwork;
using CNetwork.Sessions;
using CNetwork.Utils;
using DotNetty.Buffers;

namespace ChatServer.Network.Packets
{
    public class GetNotificationsResponse : IPacket
    {
        public List<AbstractNotification> Notifications { get; set; } = new List<AbstractNotification>();

        public void Decode(IByteBuffer buffer)
        {
        }

        public IByteBuffer Encode(IByteBuffer byteBuf)
        {
            byteBuf.WriteInt(Notifications.Count);
            foreach (var noti in Notifications)
            {
                byteBuf.WriteByte((int) noti.Type());
                noti.EncodeToBuffer(byteBuf);
            }
            return byteBuf;
        }

        public void Handle(ISession session)
        {
            throw new NotImplementedException();
        }
    }
}

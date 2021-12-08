using System;
using System.Collections.Generic;
using CNetwork;
using CNetwork.Sessions;
using CNetwork.Utils;
using DotNetty.Buffers;

namespace UI.Network.Packets.AfterLoginRequest.Notification
{
    public class GetNotificationsResult : IPacket
    {
        public List<string> Notifications = new List<string>();

        public void Decode(IByteBuffer buffer)
        {
            string noti = ByteBufUtils.ReadUTF8(buffer);
            while (!noti.Equals("~"))
            {
                Notifications.Add(noti);
                noti = ByteBufUtils.ReadUTF8(buffer);
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

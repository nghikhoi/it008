using System;
using System.Collections.Generic;
using System.Windows;
using CNetwork;
using CNetwork.Sessions;
using CNetwork.Utils;
using DotNetty.Buffers;
using UI.MVC;
using UI.Utils;

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
            Application.Current.Dispatcher.Invoke(() =>
            {
                ChatWindow module = ModuleContainer.GetModule<ChatWindow>();
                foreach (var noti in Notifications)
                {
                    NotificationInfo analyzed = NotificationDecoder.Analyze(noti);
                    
                    if (analyzed.Prefix.Equals(NotificationPrefixes.AddFriend))
                    {
                        module.controller.AddFriendRequestNoti(analyzed.SenderID, analyzed.Name);
                    }

                    if (analyzed.Prefix.Equals(NotificationPrefixes.AcceptedFriend))
                    {
                        module.controller.AddFriendAcceptedNoti(analyzed.SenderID, analyzed.Name);
                    }
                }
            });
        }
    }
}

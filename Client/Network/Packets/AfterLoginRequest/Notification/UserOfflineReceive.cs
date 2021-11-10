using System;
using System.Windows;
using CNetwork;
using CNetwork.Sessions;
using CNetwork.Utils;
using DotNetty.Buffers;

namespace UI.Network.Packets.AfterLoginRequest.Notification
{
    public class UserOfflineReceive : IPacket
    {
        public string UserID { get; set; }

        public void Decode(IByteBuffer buffer)
        {
            UserID = ByteBufUtils.ReadUTF8(buffer);
        }

        public IByteBuffer Encode(IByteBuffer byteBuf)
        {
            throw new NotImplementedException();
        }

        public void Handle(ISession session)
        {
            
            /*var app = MainWindow.chatApplication;

            if (app.model.IsOnline.ContainsKey(UserID))
                app.model.IsOnline[UserID] = true;

            if (app.model.UserControls.ContainsKey(UserID))
            {
                var friend = app.model.UserControls[UserID] as UserMessage;
                Application.Current.Dispatcher.Invoke(() => friend.SetOnlineStatus(false));
            }*/
        }
    }
}

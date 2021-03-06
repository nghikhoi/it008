using System;
using CNetwork;
using CNetwork.Sessions;
using CNetwork.Utils;
using DotNetty.Buffers;

namespace UI.Network.Packets.AfterLoginRequest.Notification
{
    public class UserOnlineReceive : IPacket
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
            /*var friend = app.model.UserControls[UserID] as UserMessage;

            if (app.model.IsOnline.ContainsKey(UserID))
                app.model.IsOnline[UserID] = false;

            Application.Current.Dispatcher.Invoke(() => friend.SetOnlineStatus(true));*/
        }
    }
}

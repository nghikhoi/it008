using System;
using System.Windows;
using CNetwork;
using CNetwork.Sessions;
using CNetwork.Utils;
using DotNetty.Buffers;
using UI.MVC;

namespace UI.Network.Packets.AfterLoginRequest.Notification
{
    public class FriendRequestReceive : IPacket
    {
        public string SenderID { get; set; }
        public string Name { get; set; }

        public void Decode(IByteBuffer buffer)
        {
            SenderID = ByteBufUtils.ReadUTF8(buffer);
            Name = ByteBufUtils.ReadUTF8(buffer);
        }

        public IByteBuffer Encode(IByteBuffer byteBuf)
        {
            throw new NotImplementedException();
        }

        public void Handle(ISession session)
        {
            Console.WriteLine("Friend request received");
            Application.Current.Dispatcher.Invoke(() => {
                ChatWindow module = ModuleContainer.GetModule<ChatWindow>();
                module.controller.AddFriendRequestNoti(SenderID, Name);
            });
        }
    }
}

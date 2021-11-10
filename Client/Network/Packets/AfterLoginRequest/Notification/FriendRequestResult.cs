﻿using System;
using System.Windows;
using CNetwork;
using CNetwork.Sessions;
using CNetwork.Utils;
using DotNetty.Buffers;
using UI.MVC;

namespace UI.Network.Packets.AfterLoginRequest.Notification
{
    public class FriendRequestResult : IPacket
    {
        public string UserID { get; set; }
        public string Name { get; set; }

        public void Decode(IByteBuffer buffer)
        {
            UserID = ByteBufUtils.ReadUTF8(buffer);
            Name = ByteBufUtils.ReadUTF8(buffer);
        }

        public IByteBuffer Encode(IByteBuffer byteBuf)
        {
            throw new NotImplementedException();
        }

        public void Handle(ISession session)
        {
            // Notify
            Application.Current.Dispatcher.Invoke(() => {
                ChatWindow module = ModuleContainer.GetModule<ChatWindow>();
                module.controller.AddFriendAcceptedNoti(UserID, Name);
            });

            Utils.Packets.SendPacket<GetFriendIDs>();   
        }
    }
}

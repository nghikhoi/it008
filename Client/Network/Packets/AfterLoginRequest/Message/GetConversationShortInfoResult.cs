﻿using System;
using CNetwork;
using CNetwork.Sessions;
using CNetwork.Utils;
using DotNetty.Buffers;
using UI.Models;
using UI.MVC;

namespace UI.Network.Packets.AfterLoginRequest.Message
{
    public class GetConversationShortInfoResult : IPacket
    {
        public string ConversationID { get; set; }
        public string ConversationName { get; set; }
        public long LastActive { get; set; }

        public void Decode(IByteBuffer buffer)
        {
            ConversationID = ByteBufUtils.ReadUTF8(buffer);
            ConversationName = ByteBufUtils.ReadUTF8(buffer);
            LastActive = buffer.ReadLong();
        }

        public IByteBuffer Encode(IByteBuffer byteBuf)
        {
            throw new NotImplementedException();
        }

        public void Handle(ISession session)
        {
            Console.WriteLine("RecvConv: " + ConversationName);
            Conversation conversation = null; //TODO
            var module = ModuleContainer.GetModule<ChatContainer>();
            module.controller.AddConversation(conversation);
            /*Application.Current.Dispatcher.Invoke(() =>
            {
                UserList.Instance.AddConversationToRecent(ConversationID, ConversationName, LastActive);
            });*/
        }
    }
}
﻿using System;
using System.Windows;
using CNetwork;
using CNetwork.Sessions;
using CNetwork.Utils;
using DotNetty.Buffers;
using UI.Models;
using UI.MVC;

namespace UI.Network.Packets.AfterLoginRequest.Message
{
    public class ReceiveMessage : IPacket
    {
        public string ConversationID { get; set; }
        public string SenderID { get; set; }
        public int PreviewCode { get; set; }
        public message Message { get; set; }

        public void Decode(IByteBuffer buffer)
        {
            ConversationID = ByteBufUtils.ReadUTF8(buffer);
            SenderID = ByteBufUtils.ReadUTF8(buffer);
            PreviewCode = buffer.ReadInt();

            //TODO
            /*switch (PreviewCode)
            {
                case 1:
                    Message = new AttachmentMessage();
                    (Message as AttachmentMessage).DecodeFromBuffer(buffer);
                    break;
                case 2:
                    Message = new ImageMessage();
                    (Message as ImageMessage).DecodeFromBuffer(buffer);
                    break;
                case 3:
                    Message = new StickerMessage();
                    (Message as StickerMessage).DecodeFromBuffer(buffer);
                    break;
                case 4:
                    Message = new TextMessage();
                    (Message as TextMessage).DecodeFromBuffer(buffer);
                    break;
                case 5:
                    Message = new VideoMessage();
                    (Message as VideoMessage).DecodeFromBuffer(buffer);
                    break;
            }*/
        }

        public IByteBuffer Encode(IByteBuffer byteBuf)
        {
            return byteBuf;
        }

        public void Handle(ISession session)
        {
            Console.WriteLine("Received" + ConversationID);

            Application.Current.Dispatcher.Invoke(() =>
            {
                var module = ModuleContainer.GetModule<ChatContainer>();
                module.controller.AddMessage(Message);
                /*var app = MainWindow.chatApplication;
                if (!app.model.Conversations.ContainsKey(ConversationID))
                {
                    app.model.Conversations.Add(ConversationID, new Utils.ConversationBubble());
                    var temp = app.model.Conversations[ConversationID];
                    temp.Members.Add(SenderID);
                }

                app.model.Conversations[ConversationID].Bubbles.Add(new Utils.BubbleInfo(Message, true));
                app.model.PrivateConversations[SenderID] = ConversationID;

                if (app.model.currentSelectedConversation.CompareTo(ConversationID) == 0)
                {
                    if (app.model.SelfID.Equals(SenderID))
                    {
                        ChatPage.Instance.SendMessage(Message, true);
                    }
                    else
                    {
                        ChatPage.Instance.SendLeftMessages(Message, false);
                        (app.model.UserControls[SenderID] as UserMessage).IncomingMessage(Message, true);
                    }
                }
                else if (!app.model.SelfID.Equals(SenderID))
                    (app.model.UserControls[SenderID] as UserMessage).IncomingMessage(Message, false);*/
            });
        }
    }
}

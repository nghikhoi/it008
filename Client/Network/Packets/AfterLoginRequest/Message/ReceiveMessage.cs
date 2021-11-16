using System;
using System.Windows;
using CNetwork;
using CNetwork.Sessions;
using CNetwork.Utils;
using DotNetty.Buffers;
using UI.Models;
using UI.Models.Message;
using UI.MVC;
using UI.Utils;

namespace UI.Network.Packets.AfterLoginRequest.Message
{
    public class ReceiveMessage : IPacket
    {
        public string ConversationID { get; set; }
        public string SenderID { get; set; }
        public int PreviewCode { get; set; }
        public AbstractMessage Message { get; set; }

        public void Decode(IByteBuffer buffer)
        {
            ConversationID = ByteBufUtils.ReadUTF8(buffer);
            SenderID = ByteBufUtils.ReadUTF8(buffer);
            PreviewCode = buffer.ReadInt();
            Message = MessageUtil.createMessage(PreviewCode);
            Message.DecodeFromBuffer(buffer);
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
                var module = ModuleContainer.GetModule<ChatWindow>();
                module.controller.AddMessage(ConversationID, SenderID, Message);
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

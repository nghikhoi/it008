using System;
using System.Collections.Generic;
using System.Linq;
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
    public class ConversationFromIDResult : IPacket
    {
        public string ConversationID { get; set; }
        public string ConversationName { get; set; }
        public int StatusCode { get; set; }
        public long LastActive { get; set; }
        public HashSet<string> Members { get; set; } = new HashSet<string>();
        public int LastMessID { get; set; }
        public int LastMediaID { get; set; }
        public int LastAttachmentID { get; set; }
        public int PreviewCode { get; set; }
        public int BubbleColor { get; set; }
        public string PreviewContent { get; set; }

        public void Decode(IByteBuffer buffer)
        {
            ConversationID = ByteBufUtils.ReadUTF8(buffer);
            ConversationName = ByteBufUtils.ReadUTF8(buffer);
            StatusCode = buffer.ReadInt();
            LastActive = buffer.ReadLong();

            // Get the number of members in this conversation
            string temp = ByteBufUtils.ReadUTF8(buffer);
            while (temp != "~")
            {
                Members.Add(temp);
                temp = ByteBufUtils.ReadUTF8(buffer);
            }

            LastMessID = buffer.ReadInt();
            LastMediaID = buffer.ReadInt();
            LastAttachmentID = buffer.ReadInt();
            PreviewCode = buffer.ReadInt();
            BubbleColor = buffer.ReadInt();
            PreviewContent = ByteBufUtils.ReadUTF8(buffer);
        }

        public IByteBuffer Encode(IByteBuffer byteBuf)
        {
            throw new NotImplementedException();
        }

        public void Handle(ISession session)
        {
            AbstractConversation conversation = null; //TODO
            var module = ModuleContainer.GetModule<ConversationList>();
            //module.controller.AddConversation(conversation);
            Application.Current.Dispatcher.Invoke(() =>
            {
                ChatModel model = ChatModel.Instance;
                ConversationCache modelConversation = model.Conversations[ConversationID];
                modelConversation.LastMessID = LastMessID;

                if (modelConversation.FirstTimeLoaded)
                {
                    modelConversation.FirstTimeLoaded = false;
                    modelConversation.LastMediaID = LastMediaID;
                    modelConversation.LastMediaIDBackup = LastMediaID;
                }

                modelConversation.LastAttachmentID = LastAttachmentID;
                modelConversation.ConversationName = ConversationName;
                modelConversation.Members = Members.ToList();

                modelConversation.Color = ColorUtils.IntToColor(BubbleColor);

                //if (app.model.MediaWindows.ContainsKey(ConversationID)
                //&& app.model.MediaWindows[ConversationID] != null)
                //{
                //    app.model.MediaWindows[ConversationID].Close();
                //    app.model.MediaWindows[ConversationID] = null;
                //}
                ChatContainerController controller = ModuleContainer.GetModule<ChatContainer>().controller;
                controller.LoadMessages(ConversationID);
                //TODO: update last active
            });
        }
    }
}

using System;
using System.Collections.Generic;
using CNetwork;
using CNetwork.Sessions;
using CNetwork.Utils;
using DotNetty.Buffers;
using UI.Models;

namespace UI.Network.Packets.AfterLoginRequest.Search
{
    public class SearchUserResult : IPacket
    {
        public List<UserShortInfo> Results = new List<UserShortInfo>();

        public void Decode(IByteBuffer buffer)
        {
            string id = ByteBufUtils.ReadUTF8(buffer);

            while (id != "~")
            {
                UserShortInfo result = new UserShortInfo();

                result.ID = id;
                result.FirstName = ByteBufUtils.ReadUTF8(buffer);
                result.LastName = ByteBufUtils.ReadUTF8(buffer);
                result.LastMessage = ByteBufUtils.ReadUTF8(buffer);
                result.ConversationID = ByteBufUtils.ReadUTF8(buffer);
                result.PreviewCode = buffer.ReadInt();
                result.IsOnline = buffer.ReadBoolean();
                result.LastLogout = new DateTime(buffer.ReadLong());
                result.Relationship = buffer.ReadInt();

                Results.Add(result);
                id = ByteBufUtils.ReadUTF8(buffer);
            }
        }

        public IByteBuffer Encode(IByteBuffer byteBuf)
        {
            throw new NotImplementedException();
        }

        public void Handle(ISession session)
        {
            //TODO
            /*Application.Current.Dispatcher.Invoke(() =>
            {
                if (String.IsNullOrEmpty(UserList.Instance.UserSearchBox.Text)) return;
                UserList.Instance.ClearListView();
                foreach (SearchResult result in Results)
                {
                    if (String.IsNullOrEmpty(UserList.Instance.UserSearchBox.Text)) return;
                    UserList.Instance.AddUserToListView(new UserMessageViewModel()
                    {
                        Id = result.ID,
                        Name = result.FirstName + " " + result.LastName,
                        IsOnline = result.IsOnline,
                        IncomingMsg = result.PreviewCode == -1 ? String.Empty : (result.PreviewCode == 4 ? result.LastMessage : TranslatedPreviewCode[result.PreviewCode])
                    }, result.Relationship == 2);

                    if (result.Relationship == 2)
                    {
                        var app = MainWindow.chatApplication;
                        if (!app.model.Conversations.ContainsKey(result.ConversationID))
                            app.model.Conversations.Add(result.ConversationID, new ConversationBubble());

                        app.model.Conversations[result.ConversationID].Members.Clear();
                        app.model.Conversations[result.ConversationID].Members.Add(result.ID);

                        if (!app.model.PrivateConversations.ContainsKey(result.ID))
                            app.model.PrivateConversations.Add(result.ID, result.ConversationID);

                        if (!app.model.IsOnline.ContainsKey(result.ID))
                            app.model.IsOnline.Add(result.ID, result.IsOnline);
                    }
                }
            });*/
        }


        private readonly string[] TranslatedPreviewCode = new string[6]
        {
            "Hidden message",
            "Attachment",
            "Image message",
            "Sticker",
            "-",
            "Video"
        };
    }
    
}

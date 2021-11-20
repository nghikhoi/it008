using System;
using System.Windows;
using CNetwork;
using CNetwork.Sessions;
using CNetwork.Utils;
using DotNetty.Buffers;
using UI.Models;
using UI.MVC;

namespace UI.Network.Packets.AfterLoginRequest
{
    public class GetShortInfoResult : IPacket {

        public UserShortInfo info;
        public string ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string LastMessage { get; set; }
        public string ConversationID { get; set; }
        public int PreviewCode { get; set; }
        public bool IsOnline { get; set; }
        public DateTime LastLogout { get; set; }
        public int Relationship { get; set; }

        private readonly string[] TranslatedPreviewCode = new string[6]
        {
            "Hidden message",
            "Attachment",
            "Image message",
            "Sticker",
            "-",
            "Video"
        };

        public void Decode(IByteBuffer buffer)
        {
            info.ID = ByteBufUtils.ReadUTF8(buffer);
            info.FirstName = ByteBufUtils.ReadUTF8(buffer);
            info.LastName = ByteBufUtils.ReadUTF8(buffer);
            info.LastMessage = ByteBufUtils.ReadUTF8(buffer);
            info.ConversationID = ByteBufUtils.ReadUTF8(buffer);
            info.PreviewCode = buffer.ReadInt();
            info.IsOnline = buffer.ReadBoolean();
            info.LastLogout = new DateTime(buffer.ReadLong());
            info.Relationship = buffer.ReadInt();

            Console.WriteLine(FirstName + " " + LastName);
        }

        public IByteBuffer Encode(IByteBuffer byteBuf)
        {
            throw new NotImplementedException();
        }

        public void Handle(ISession session)
        {
            var id = ID;
            Console.WriteLine(Relationship); 

            //TODO
            /*Application.Current.Dispatcher.Invoke(() => UserList.Instance.AddUserToListView(
                new UserMessageViewModel()
                {
                    Id = id,
                    Name = FirstName + " " + LastName,
                    IsOnline = IsOnline,
                    IncomingMsg = PreviewCode == -1 ? String.Empty : (PreviewCode == 4 ? LastMessage : TranslatedPreviewCode[PreviewCode])
                }, Relationship == 2));*/

            if (Relationship == 2)
            {
               ChatModel model = ChatModel.Instance;
               ConversationCache cache = model.computeIfAbsent(ConversationID, new ConversationCache());

               cache.Members.Clear();
               cache.Members.Add(ID);

               if (!model.PrivateConversations.ContainsKey(ID))
                   model.PrivateConversations.Add(ID, ConversationID);

               model.updateOnlineStatus(id, IsOnline);
                  
            }
        }
    }
}

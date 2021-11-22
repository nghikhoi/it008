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

        public UserShortInfo info = new UserShortInfo();

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
        }

        public IByteBuffer Encode(IByteBuffer byteBuf)
        {
            throw new NotImplementedException();
        }

        public void Handle(ISession session)
        {
            var id = info.ID;
            ConversationList conversationList = ModuleContainer.GetModule<ConversationList>();
            conversationList.controller.addShortInfo(info);

            if (info.Relationship == 2) {
               ChatModel model = ChatModel.Instance;
               ConversationCache cache = model.computeIfAbsent(info.ConversationID, new ConversationCache());

               cache.Members.Clear();
               cache.Members.Add(info.ID);

               if (!model.PrivateConversations.ContainsKey(info.ID))
                   model.PrivateConversations.Add(info.ID, info.ConversationID);

               model.updateOnlineStatus(id, info.IsOnline);
            }
        }
    }
}

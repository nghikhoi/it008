using System.Windows;
using CNetwork;
using CNetwork.Sessions;
using CNetwork.Utils;
using DotNetty.Buffers;
using UI.MVC;

namespace UI.Network.Packets.AfterLoginRequest.Message
{
    public class SingleConversationFrUserIDResult : IPacket
    {
        public string UserID { get; set; }
        public string ConversationID { get; set; }

        public void Decode(IByteBuffer buffer)
        {
            UserID = ByteBufUtils.ReadUTF8(buffer);
            ConversationID = ByteBufUtils.ReadUTF8(buffer);
        }

        public IByteBuffer Encode(IByteBuffer byteBuf)
        {
            // throw new NotImplementedException();
            return byteBuf;
        }

        public void Handle(ISession session)
        {
            Application.Current.Dispatcher.Invoke(
            () =>
            {
                var module = ModuleContainer.GetModule<ChatContainer>();
                module.controller.LoadConversation(UserID, ConversationID);
                /*var app = MainWindow.chatApplication;
                app.model.PrivateConversations[UserID] = ConversationID;
                app.model.Conversations.Add(ConversationID, new Utils.ConversationBubble());
                app.model.Conversations[ConversationID].Members.Add(UserID);
                ChatPage.Instance.LoadChatPage(ConversationID);*/
            });
        }
    }
}

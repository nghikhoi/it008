using System;
using System.Windows;
using CNetwork;
using CNetwork.Sessions;
using CNetwork.Utils;
using DotNetty.Buffers;
using UI.Debug;
using UI.MVC;

namespace UI.Network.Packets.AfterLoginRequest.Message
{
    public class BuzzReceive : IPacket
    {
        public String SenderID { get; set; }
        public String ConversationID { get; set; }

        public void Decode(IByteBuffer buffer)
        {
            SenderID = ByteBufUtils.ReadUTF8(buffer);
            ConversationID = ByteBufUtils.ReadUTF8(buffer);
        }

        public IByteBuffer Encode(IByteBuffer byteBuf)
        {
            throw new NotImplementedException();
        }

        public void Handle(ISession session) {
            var module = ModuleContainer.GetModule<ChatContainer>();
            Application.Current.Dispatcher.Invoke(() => 
            {
                module.controller.Buzz();
                Logger.log(ConversationID);
            });
        }
    }
}

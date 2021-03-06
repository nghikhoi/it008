using System;
using CNetwork;
using CNetwork.Sessions;
using CNetwork.Utils;
using DotNetty.Buffers;
using UI.Views;

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
            ChatPage.DoPublicBuzz();
        }
    }
}

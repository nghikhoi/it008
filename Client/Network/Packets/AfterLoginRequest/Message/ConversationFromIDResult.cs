using System;
using System.Collections.Generic;
using CNetwork;
using CNetwork.Sessions;
using CNetwork.Utils;
using DotNetty.Buffers;

namespace UI.Network.Packets.AfterLoginRequest.Message
{
    public class ConversationFromIDResult : IPacket
    {
        public string ConversationID { get; set; }
        public string ConversationName { get; set; }
        public int StatusCode { get; set; }
        public long LastActive { get; set; }
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
        }
    }
}

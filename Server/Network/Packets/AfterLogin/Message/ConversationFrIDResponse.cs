using System;
using System.Collections.Generic;
using CNetwork;
using CNetwork.Sessions;
using CNetwork.Utils;
using DotNetty.Buffers;

namespace ChatServer.Network.Packets
{
    public class ConversationFrIDResponse : IPacket
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
            throw new NotImplementedException();
        }

        public IByteBuffer Encode(IByteBuffer byteBuf)
        {
            ByteBufUtils.WriteUTF8(byteBuf, ConversationID); 
            ByteBufUtils.WriteUTF8(byteBuf, ConversationName); 
            byteBuf.WriteInt(StatusCode);
            byteBuf.WriteLong(LastActive);

            byteBuf.WriteInt(LastMessID);
            byteBuf.WriteInt(LastMediaID);
            byteBuf.WriteInt(LastAttachmentID);
            byteBuf.WriteInt(PreviewCode);
            byteBuf.WriteInt(BubbleColor);
            ByteBufUtils.WriteUTF8(byteBuf, PreviewContent);

            return byteBuf;
        }

        public void Handle(ISession session)
        {
            throw new NotImplementedException();
        }
    }
}

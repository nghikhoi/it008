using System;
using System.Collections.Generic;
using ChatServer.Entity;
using CNetwork;
using CNetwork.Sessions;
using CNetwork.Utils;
using DotNetty.Buffers;

namespace ChatServer.Network.Packets
{
    public class MessageFromConversationResponse : IPacket
    {
        //Type of message
        /// <summary>
        /// 1: Attachment
        /// 2: Image
        /// 3: Sticker
        /// 4: Text
        /// 5: Video
        /// </summary>

        public List<string> SenderID { get; set; } = new List<string>();

        // Text content is content of text message (if preview code is equal to 4)
        public List<AbstractMessage> Content { get; set; } = new List<AbstractMessage>();

        public bool LoadConversation { get; set; }

        public void Decode(IByteBuffer buffer)
        {
            throw new NotImplementedException();
        }

        public IByteBuffer Encode(IByteBuffer byteBuf)
        {
            for (int i = 0; i < SenderID.Count; ++i)
            {
                ByteBufUtils.WriteUTF8(byteBuf, SenderID[i]);
                byteBuf = Content[i].EncodeToBuffer(byteBuf);
            }
            ByteBufUtils.WriteUTF8(byteBuf, "~");
            byteBuf.WriteBoolean(LoadConversation);
            return byteBuf;
        }

        public void Handle(ISession session)
        {
            throw new NotImplementedException();
        }
    }
}

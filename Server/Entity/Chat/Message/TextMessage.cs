using System;
using CNetwork.Utils;
using DotNetty.Buffers;
using MongoDB.Bson.Serialization.Attributes;

namespace ChatServer.Entity
{
    public class TextMessage : AbstractMessage
    {
        [BsonElement("Text")]
        public String Message { get; set; }

        public override void DecodeFromBuffer(IByteBuffer buffer)
        {
            Message = ByteBufUtils.ReadUTF8(buffer);
        }

        public override IByteBuffer EncodeToBuffer(IByteBuffer buffer)
        {
            buffer.WriteInt(GetPreviewCode());
            ByteBufUtils.WriteUTF8(buffer, Message);
            return buffer;
        }

        public override int GetPreviewCode()
        {
            return 4;
        }
    }
}

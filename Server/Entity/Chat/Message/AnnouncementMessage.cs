using System;
using ChatServer.Utils;
using CNetwork.Utils;
using DotNetty.Buffers;
using MongoDB.Bson.Serialization.Attributes;

namespace ChatServer.Entity
{

    public enum AnnouncementType {
        CHANGE_AVATAR, CHANGE_NICKNAME, ADD_MEMBER
    }

    public class AnnouncementMessage : AbstractMessage
    {
        [BsonElement(nameof(Type))]
        public AnnouncementType Type { get; set; }
        [BsonElement(nameof(Value))]
        public String Value { get; set; }

        public override void DecodeFromBuffer(IByteBuffer buffer)
        {
            Type = (AnnouncementType) buffer.ReadInt();
            Value = ByteBufUtils.ReadUTF8(buffer);
        }

        public override IByteBuffer EncodeToBuffer(IByteBuffer buffer)
        {
            buffer.WriteInt(GetPreviewCode());
            buffer.WriteInt((int) Type);
            ByteBufUtils.WriteUTF8(buffer, FastCodeUtils.NotEmptyStrings(Value) ? Value : "");
            return buffer;
        }

        public override int GetPreviewCode()
        {
            return 6;
        }
    }
}

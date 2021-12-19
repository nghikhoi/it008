using System;
using CNetwork.Utils;
using DotNetty.Buffers;
using UI.Utils;

namespace UI.Models.Message
{

    public enum AnnouncementType {
        CHANGE_AVATAR, CHANGE_NICKNAME, ADD_NEW_MEMBER
    }

    public class AnnouncementMessage : AbstractMessage
    {
        public AnnouncementType Type { get; set; }
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

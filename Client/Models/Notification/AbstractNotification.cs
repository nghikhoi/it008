using System;
using CNetwork.Utils;
using DotNetty.Buffers;

namespace UI.Models.Notification {
    public abstract class AbstractNotification : INotification {
        public Guid Id { get; set; }
        public Guid TargetUser { get; set; }
        public long CreatedTime { get; set; }

        public abstract NotificationType Type();

        public virtual IByteBuffer EncodeToBuffer(IByteBuffer buffer)
        {
            ByteBufUtils.WriteUTF8(buffer, Id.ToString());
            ByteBufUtils.WriteUTF8(buffer, TargetUser.ToString());
            ByteBufUtils.WriteVarLong(buffer, CreatedTime);
            return buffer;
        }

        public virtual void DecodeFromBuffer(IByteBuffer buffer)
        {
            Id = Guid.Parse(ByteBufUtils.ReadUTF8(buffer));
            TargetUser = Guid.Parse(ByteBufUtils.ReadUTF8(buffer));
            CreatedTime = ByteBufUtils.ReadVarLong(buffer);
        }
    }
}

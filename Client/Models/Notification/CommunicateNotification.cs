using System;
using CNetwork.Utils;
using DotNetty.Buffers;

namespace UI.Models.Notification {
    public abstract class CommunicateNotification : AbstractNotification {
        public Guid SenderUser { get; set; }
        public string SenderName { get; set; }

        public override IByteBuffer EncodeToBuffer(IByteBuffer buffer) {
            IByteBuffer buf = base.EncodeToBuffer(buffer);
            ByteBufUtils.WriteUTF8(buf, SenderUser.ToString());
            ByteBufUtils.WriteUTF8(buf, SenderName);
            return buf;
        }

        public override void DecodeFromBuffer(IByteBuffer buffer) {
            base.DecodeFromBuffer(buffer);
            SenderUser = Guid.Parse(ByteBufUtils.ReadUTF8(buffer));
            SenderName = ByteBufUtils.ReadUTF8(buffer);
        }
    }
}

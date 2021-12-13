using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CNetwork.Utils;
using DotNetty.Buffers;
using MongoDB.Bson.Serialization.Attributes;

namespace ChatServer.Entity.Notification {
    [BsonKnownTypes(typeof(AcceptFriendNotification), typeof(FriendRequestNotification))]
    public abstract class CommunicateNotification : AbstractNotification {

        [BsonElement(nameof(SenderUser))]
        public Guid SenderUser { get; set; }
        [BsonElement(nameof(SenderName))]
        public string SenderName { get; set; }

        public override IByteBuffer EncodeToBuffer(IByteBuffer buffer)
        {
            IByteBuffer buf = base.EncodeToBuffer(buffer);
            ByteBufUtils.WriteUTF8(buf, SenderUser.ToString());
            ByteBufUtils.WriteUTF8(buf, SenderName ?? "Null");
            return buf;
        }

        public override void DecodeFromBuffer(IByteBuffer buffer)
        {
            base.DecodeFromBuffer(buffer);
            SenderUser = Guid.Parse(ByteBufUtils.ReadUTF8(buffer));
            SenderName = ByteBufUtils.ReadUTF8(buffer);
        }
    }
}

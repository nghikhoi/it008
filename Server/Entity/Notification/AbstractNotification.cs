using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CNetwork.Utils;
using DotNetty.Buffers;
using MongoDB.Bson.Serialization.Attributes;

namespace ChatServer.Entity.Notification {
    [BsonKnownTypes(typeof(CommunicateNotification))]
    public abstract class AbstractNotification : INotification {
        [BsonId]
        public Guid Id { get; set; }
        [BsonElement(nameof(TargetUser))]
        public Guid TargetUser { get; set; }
        [BsonElement(nameof(CreatedTime))]
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

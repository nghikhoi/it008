using System;
using CNetwork.Utils;
using DotNetty.Buffers;
using MongoDB.Bson.Serialization.Attributes;

namespace ChatServer.Entity
{
    public class AttachmentMessage : FileMessage {
        public override int GetPreviewCode()
        {
            return 1;
        }
    }
}

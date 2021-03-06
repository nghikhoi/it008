using System;
using System.Collections.Generic;
using DotNetty.Buffers;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;

namespace ChatServer.Entity
{
    /// <summary>
    /// Pattern:
    ///     - Template Method
    ///     - Decorator: make the change role/type of message easily
    /// </summary>
    [BsonKnownTypes(typeof(AttachmentMessage), typeof(ImageMessage), typeof(StickerMessage), typeof(TextMessage), typeof(VideoMessage), typeof(AnnouncementMessage))]
    public abstract class AbstractMessage : IMessage
    {
        [BsonId]
        public Guid ID { get; set; }

        //ID of sender, sender is user
        [BsonElement("Author")]
        public Guid Author { get; set; }

        //List of user seen this message
        [BsonElement("Seens")]
        public HashSet<Guid> Seens { get; private set; } = new HashSet<Guid>();

        //if Revoked = true, the message will be hidden to all users.
        [BsonElement("Revoked")]
        public bool Revoked { get; set; } = false;

        //IDs of users who hide this message away from their conversation, the message still show to the others
        [BsonElement("Hide")]
        public HashSet<Guid> Hide { get; private set; } = new HashSet<Guid>();

        //IDs of user who reacted and react id;
        [BsonElement("Reacts"), BsonDictionaryOptions(DictionaryRepresentation.ArrayOfDocuments)]
        public Dictionary<Guid, int> Reacts { get; private set; } = new Dictionary<Guid, int>();

        /// <summary>
        /// 1: Attachment
        /// 2: Image
        /// 3: Sticker
        /// 4: Text
        /// 5: Video
        /// </summary>
        public abstract int GetPreviewCode();
        public abstract IByteBuffer EncodeToBuffer(IByteBuffer buffer);
        public abstract void DecodeFromBuffer(IByteBuffer buffer);

        public bool Showable(Guid id)
        {
            return !Revoked && !Hide.Contains(id);
        }
    }
}

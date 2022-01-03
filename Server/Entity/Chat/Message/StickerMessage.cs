using DotNetty.Buffers;
using MongoDB.Bson.Serialization.Attributes;

namespace ChatServer.Entity
{
    public class StickerMessage : AbstractMessage
    {
        [BsonElement("CategoryID")]
        public int CategoryID { get; set; }
        [BsonElement("StickerID")]
        public int StickerID { get; set; }

        public override void DecodeFromBuffer(IByteBuffer buffer)
        {
            StickerID = buffer.ReadInt();
            CategoryID = buffer.ReadInt();
        }

        public override IByteBuffer EncodeToBuffer(IByteBuffer buffer)
        {
            buffer.WriteInt(GetPreviewCode());
            buffer.WriteInt(StickerID);
            buffer.WriteInt(CategoryID);
            return buffer;
        }

        public override int GetPreviewCode()
        {
            return 3;
        }
    }
}

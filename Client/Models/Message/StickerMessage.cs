using DotNetty.Buffers;

namespace UI.Models.Message
{
    public class StickerMessage : AbstractMessage
    {
        public Sticker Sticker { get; set; } = new Sticker();

        public override void DecodeFromBuffer(IByteBuffer buffer)
        {
            Sticker.ID = buffer.ReadInt();
            Sticker.CategoryID = buffer.ReadInt();
        }

        public override IByteBuffer EncodeToBuffer(IByteBuffer buffer)
        {
            buffer.WriteInt(GetPreviewCode());
            buffer.WriteInt(Sticker.ID);
            buffer.WriteInt(Sticker.CategoryID);
            return buffer;
        }

        public override int GetPreviewCode()
        {
            return 3;
        }
    }
}

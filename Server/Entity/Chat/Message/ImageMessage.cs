namespace ChatServer.Entity
{
    public class ImageMessage : FileMessage {
        public override int GetPreviewCode()
        {
            return 2;
        }
    }
}

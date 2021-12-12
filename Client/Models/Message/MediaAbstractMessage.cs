namespace UI.Models.Message
{
    public abstract class MediaAbstractMessage : AbstractMessage
    {
        public string FileName { get; set; }
        public string FileID { get; set; }
    }
}

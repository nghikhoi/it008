using System.Collections.Generic;
using System.Windows.Media;
using UI.Models;
using UI.Models.Message;

namespace UI.Utils
{
    public class BubbleInfo
    {
        public AbstractMessage Message;
        public bool OnLeft;

        public BubbleInfo(AbstractMessage msg, bool lft)
        {
            Message = msg;
            OnLeft = lft;
        }
    }

    public class ConversationBubble
    {
        public List<string> Members { get; set; } = new List<string>();
        public int LastMessID { get; set; }
        public int LastMediaID { get; set; }
        public int LastMediaIDBackup { get; set; }
        public int LastAttachmentID { get; set; }
        public Color Color { get; set; }
        public string ConversationName { get; set; }
        public bool FirstTimeLoaded { get; set; } = true;
        public List<BubbleInfo> Bubbles = new List<BubbleInfo>();
    }

    public enum BubbleType
    {
        Attachment = 1,
        Image = 2,
        Sticker = 3, 
        Text = 4,
        Video = 5
    }

    public class BubbleTypeParser
    {
        public static BubbleType Parse(AbstractMessage message)
        {
            return (BubbleType) message.GetPreviewCode();
        }
    }
}

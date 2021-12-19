using System;
using System.Collections.Generic;
using System.Windows.Media;
using UI.Models.Message;

namespace UI.Models {

    public enum BubbleType
	{
		Attachment = 1,
		Image = 2,
		Sticker = 3, 
		Text = 4,
		Video = 5,
		Announcement = 6
	}

	public class BubbleTypeParser
	{
		public static BubbleType Parse(AbstractMessage message)
		{
			return (BubbleType) message.GetPreviewCode();
		}
	}
	
}
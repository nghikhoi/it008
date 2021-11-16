using UI.Models.Message;

namespace UI.Utils {
	
	public class MessageUtil {

		public static AbstractMessage createMessage(int type) {
			switch (type)
			{
			    case 1:
				    return new AttachmentMessage();
			    case 2:
				    return new ImageMessage();
			    case 3:
				    return new StickerMessage();
			    case 4:
				    return new TextMessage();
			    case 5:
				    return new VideoMessage();
			}
			return null;
		}
		
	}
	
}
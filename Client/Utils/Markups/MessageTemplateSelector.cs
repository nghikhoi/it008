using System.Windows;
using System.Windows.Controls;
using UI.Models;
using UI.Models.Message;
using UI.ViewModels;

namespace UI.Utils.Markups {
	public class MessageTemplateSelector : DataTemplateSelector {
		
		public override DataTemplate SelectTemplate(object item, DependencyObject container) {
			FrameworkElement element = container as FrameworkElement;
			if (element != null && item != null && item is MessageViewModel) {
				AbstractMessage message = (item as MessageViewModel).Message;
				BubbleType type = BubbleTypeParser.Parse(message);

				switch (type) {
					case BubbleType.Attachment: {
						return element.FindResource("AttachmentMessage") as DataTemplate;
					}
					case BubbleType.Image: {
						return element.FindResource("ImageMessage") as DataTemplate;
					}
					case BubbleType.Sticker: {
						return element.FindResource("StickerMessage") as DataTemplate;
					}
					case BubbleType.Text: {
						return element.FindResource("TextMessage") as DataTemplate;
					}
					case BubbleType.Emoji:
						{
					return element.FindResource("Emoji") as DataTemplate;
						}
					case BubbleType.Video: {
						return element.FindResource("VideoMessage") as DataTemplate;
					}
				}
			}
			return null;
		}
		
	}
}
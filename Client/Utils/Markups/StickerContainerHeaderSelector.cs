using System.Windows;
using System.Windows.Controls;
using UI.Models;
using UI.Models.Message;
using UI.ViewModels;

namespace UI.Utils.Markups {
	public class StickerContainerHeaderSelector : DataTemplateSelector {
		
		public override DataTemplate SelectTemplate(object item, DependencyObject container) {
			FrameworkElement element = container as FrameworkElement;
			if (element != null && item != null) {
                if (item is StickerStoreViewModel)
                {
                    return element.FindResource("StickerStoreHeader") as DataTemplate;
                }

                if (item is StickerRecentTabViewModel)
                {
                    return element.FindResource("StickerRecentTabHeader") as DataTemplate;
				}
                if (item is StickerOwnedTabViewModel)
                {
					return element.FindResource("StickerTabHeader") as DataTemplate;
                }
			}
			return null;
		}
		
	}
}
using System.Windows;
using System.Windows.Controls;
using UI.Models;
using UI.Models.Message;
using UI.ViewModels;

namespace UI.Utils.Markups {
	public class StickerContainerSelector : DataTemplateSelector {
		
		public override DataTemplate SelectTemplate(object item, DependencyObject container) {
			FrameworkElement element = container as FrameworkElement;
			if (element != null && item != null) {
                if (item is StickerStoreViewModel)
                {
                    return element.FindResource("StickerStore") as DataTemplate;
                }

                if (item is StickerRecentTabViewModel)
                {
                    return element.FindResource("StickerRecentTab") as DataTemplate;
				}
                if (item is StickerOwnedTabViewModel)
                {
					return element.FindResource("StickerTab") as DataTemplate;
                }
			}
			return null;
		}
		
	}
}
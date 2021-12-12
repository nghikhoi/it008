using System;
using System.Windows;
using System.Windows.Controls;
using UI.Models;
using UI.Models.Message;
using UI.ViewModels;

namespace UI.Utils.Markups {
	public class NotificationTemplateSelector : DataTemplateSelector {
		
		public override DataTemplate SelectTemplate(object item, DependencyObject container) {
			FrameworkElement element = container as FrameworkElement;
			if (element != null && item != null && item is NotificationViewModel) {
				NotificationInfo info = (item as NotificationViewModel).Info;

				if (string.CompareOrdinal(info.Prefix, NotificationPrefixes.AcceptedFriend) == 0) {
					return element.FindResource("AddFriendNotification") as DataTemplate;
				}
				if (string.CompareOrdinal(info.Prefix, NotificationPrefixes.AddFriend) == 0) {
					return element.FindResource("FriendAccepectNotification") as DataTemplate;
				}
			}
			return null;
		}
		
	}
}
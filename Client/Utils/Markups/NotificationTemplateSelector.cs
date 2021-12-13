using System;
using System.Windows;
using System.Windows.Controls;
using UI.Models;
using UI.Models.Message;
using UI.Models.Notification;
using UI.ViewModels;

namespace UI.Utils.Markups {
	public class NotificationTemplateSelector : DataTemplateSelector {
		
		public override DataTemplate SelectTemplate(object item, DependencyObject container) {
			FrameworkElement element = container as FrameworkElement;
			if (element != null && item != null && item is NotificationViewModel) {
				AbstractNotification info = (item as NotificationViewModel).Info;

                switch (info.Type())
                {
					case NotificationType.FRIEND_REQUEST:
                        return element.FindResource("AddFriendNotification") as DataTemplate;
					case NotificationType.ACCEPT_FRIEND_RESPONDE:
                        return element.FindResource("FriendAccepectNotification") as DataTemplate;
				}
			}
			return null;
		}
		
	}
}
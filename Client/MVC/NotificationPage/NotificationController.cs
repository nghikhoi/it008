using System.Windows.Controls;
using UI.Network;
using UI.Network.Packets.AfterLoginRequest.Notification;
using UI.Network.RestAPI;
using UI.Utils;

namespace UI.MVC {
	
	public partial class NotificationController : IController {

		private NotificationPage view;

		public NotificationController(NotificationPage view) {
			this.view = view;
		}
		
		public void initNotifications() {
			DataAPI.getData<GetNotifications, GetNotificationsResult>(result => {
				foreach (string notification in result.Notifications) {
					NotificationInfo analyzed = NotificationDecoder.Analyze(notification);
					addNotification(analyzed);
				}
			});
		}

		public void addNotification(NotificationInfo info) {
			//TODO display lên view
			string prefix = info.Prefix;
			if (string.CompareOrdinal(prefix, NotificationPrefixes.AcceptedFriend) == 0) {
				FriendRequestNoti fri = new FriendRequestNoti();
				fri.FriendName.Text = info.Name;
				fri.ID = info.SenderID;
				StackPanel viewFriendRequestContainer = this.view.FriendRequestContainer;
				int position = viewFriendRequestContainer.Children.Count;
				fri.AcceptClick += (s, a) => {
					respondeFriendRequest(position, info.SenderID, true);
					viewFriendRequestContainer.Children.Remove(fri);
				};
				fri.DenyClick += (s, a) => {
					respondeFriendRequest(position, info.SenderID, false);
					viewFriendRequestContainer.Children.Remove(fri);
				};
				viewFriendRequestContainer.Children.Add(fri);
			} else if (string.CompareOrdinal(prefix, NotificationPrefixes.AddFriend) == 0) {
				NotificationTagItem item = new NotificationTagItem();
				item.Content = info.Title;
				view.NewestContainer.Children.Add(item);
			}
		}

		public void respondeFriendRequest(int index, string userId, bool accept) {
			ResponseFriendRequest responde = new ResponseFriendRequest();
			responde.NotiPosition = index;
			responde.TargetID = userId;
			responde.Accepted = accept;
			ChatConnection.Instance.Send(responde);
		}

	}

}
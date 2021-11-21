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
			FriendRequestNoti fri = new FriendRequestNoti();
			fri.FriendName.Text = info.Name;
			fri.ID = info.SenderID;
			this.view.NotificationContainer.Children.Add(fri);
		}

	}

}
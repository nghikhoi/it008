using UI.Lang;
using UI.Models.Notification;
using UI.Network;

namespace UI.ViewModels {
	public class FriendAccpectedNotificationViewModel : NotificationViewModel {

		#region Properties

        public new AcceptFriendNotification Info {
            get => (AcceptFriendNotification) base.Info;
            set {
                base.Info = value;
            }
        }

        public string SenderName {
            get => Info.SenderName;
            set {
                Info.SenderName = value;
                OnPropertyChanged(nameof(SenderName));
            }
        }

        public string Content
        {
            get => SenderName + " đã chấp nhận lời mời kết bạn!";
        }

		#endregion

		private readonly ChatConnection _connection;

		public FriendAccpectedNotificationViewModel(ChatConnection connection) {
			_connection = connection;
		}
		
		
	}
}
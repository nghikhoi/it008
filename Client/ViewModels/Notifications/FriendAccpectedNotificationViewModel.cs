using UI.Network;

namespace UI.ViewModels {
	public class FriendAccpectedNotificationViewModel : NotificationViewModel {

		#region Properties

		public string Content {
			get => Info.Title;
			set {
				Info.Title = value;
				OnPropertyChanged(nameof(Content));
			}
		}
		
		public string SenderName {
			get => Info.Name;
			set {
				Info.Name = value;
				OnPropertyChanged(nameof(SenderName));
			}
		}
		
		#endregion

		private readonly ChatConnection _connection;

		public FriendAccpectedNotificationViewModel(ChatConnection connection) {
			_connection = connection;
		}
		
		
	}
}
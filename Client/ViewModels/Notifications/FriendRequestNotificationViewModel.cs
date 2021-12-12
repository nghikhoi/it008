using UI.Network;

namespace UI.ViewModels {
	public class FriendRequestNotificationViewModel : NotificationViewModel {

		#region Properties

		private int _position;
		public int Position {
			get => _position;
			set => _position = value;
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

		public FriendRequestNotificationViewModel(ChatConnection connection) {
			_connection = connection;
		}
		
		
	}
}
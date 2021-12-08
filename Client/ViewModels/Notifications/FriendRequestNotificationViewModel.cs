using System.Collections.ObjectModel;
using UI.Models.Message;
using UI.Network;
using UI.Utils;

namespace UI.ViewModels.Notifications {
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
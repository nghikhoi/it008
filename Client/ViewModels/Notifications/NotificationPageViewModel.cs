using System.Collections.ObjectModel;
using UI.Network;
using UI.Network.Packets.AfterLoginRequest.Notification;
using UI.Network.RestAPI;
using UI.Services;
using UI.Utils;

namespace UI.ViewModels {
	public class NotificationPageViewModel : InitializableViewModel {

		#region Properties

		private ObservableCollection<NotificationViewModel> _newestNotifications;
		public ObservableCollection<NotificationViewModel> NewestNotifications { get => _newestNotifications; set => _newestNotifications = value; }
		private ObservableCollection<NotificationViewModel> _notifications;
		public ObservableCollection<NotificationViewModel> Notifications { get => _notifications; set => _notifications = value; }

		#endregion

		#region Command



		#endregion

		private ChatConnection _connection;
		private IViewModelFactory _viewModelFactory;

		public NotificationPageViewModel(ChatConnection connection, IViewModelFactory viewModelFactory, PacketRespondeListener listener) {
			_connection = connection;
			_viewModelFactory = viewModelFactory;
		}

		protected override void Initialize(object parameter = null) {
			loadNotifications();
		}
		
		private void loadNotifications() {
			DataAPI.getData<GetNotifications, GetNotificationsResult>(result => {
				foreach (string notification in result.Notifications) {
					NotificationInfo analyzed = NotificationDecoder.Analyze(notification);
					AddNotification(analyzed);
				}
			});
		}

		public void AddNotification(NotificationInfo info) {
			string prefix = info.Prefix;
			if (string.CompareOrdinal(prefix, NotificationPrefixes.AcceptedFriend) == 0) {
				AddFriendAccepectedNotification(info);
			} else if (string.CompareOrdinal(prefix, NotificationPrefixes.AddFriend) == 0) {
				AddFriendNotification(info);
			}
		}

		private void AddFriendNotification(NotificationInfo info) {
			FriendRequestNotificationViewModel viewModel = _viewModelFactory.Create<FriendRequestNotificationViewModel>();
			viewModel.Info = info;
			viewModel.Position = NewestNotifications.Count;
			NewestNotifications.Add(viewModel);
		}

		private void AddFriendAccepectedNotification(NotificationInfo info) {
			FriendAccpectedNotificationViewModel viewModel = _viewModelFactory.Create<FriendAccpectedNotificationViewModel>();
			viewModel.Info = info;
			NewestNotifications.Add(viewModel);
		}

	}
}
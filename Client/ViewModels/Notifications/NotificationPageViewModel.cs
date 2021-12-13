﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using CNetwork.Sessions;
using UI.Models.Notification;
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

			NewestNotifications = new ObservableCollection<NotificationViewModel>();
			Notifications = new ObservableCollection<NotificationViewModel>();
			
            listener.ReceiveNotificationEvent += ReceiveNotification;
			InitializeCommand.Execute(null);
        }

		protected override void Initialize(object parameter = null) {
			loadNotifications();
		}

        protected void ReceiveNotification(ISession session, GetNotificationsResult result)
        {
			result.Notifications.ForEach(AddNotification);
        }
		
		private void loadNotifications() {
			DataAPI.getData<GetNotifications, GetNotificationsResult>(result => {
				foreach (AbstractNotification notification in result.Notifications) {
					AddNotification(notification);
				}
			});
		}

		public void AddNotification(AbstractNotification info) {
            switch (info.Type())
            {
                case NotificationType.ACCEPT_FRIEND_RESPONDE:
                {
					AddFriendAccepectedNotification(info);
                    break;
                }
                case NotificationType.FRIEND_REQUEST:
                {
					AddFriendNotification(info);
                    break;
                }
				default: return;
            }
		}

		private void AddFriendNotification(AbstractNotification info) {
			FriendRequestNotificationViewModel viewModel = _viewModelFactory.Create<FriendRequestNotificationViewModel>();
			viewModel.Info = (FriendRequestNotification) info;
			NewestNotifications.Add(viewModel);
		}

		private void AddFriendAccepectedNotification(AbstractNotification info) {
			FriendAccpectedNotificationViewModel viewModel = _viewModelFactory.Create<FriendAccpectedNotificationViewModel>();
			viewModel.Info = (AcceptFriendNotification) info;
			NewestNotifications.Add(viewModel);
		}

	}
}
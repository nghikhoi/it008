using System;
using System.Windows.Input;
using UI.Command;
using UI.Models.Message;
using UI.Models.Notification;
using UI.Network;
using UI.Network.Packets.AfterLoginRequest.Notification;

namespace UI.ViewModels {
	public class FriendRequestNotificationViewModel : NotificationViewModel {

		#region Properties

        public new FriendRequestNotification Info {
            get => (FriendRequestNotification) base.Info;
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
		
		public ICommand RespondeCommand { get; set; }

		#endregion

		private readonly ChatConnection _connection;
        public event Action<bool> OnResponse;

		public FriendRequestNotificationViewModel(ChatConnection connection) {
			_connection = connection;
            RespondeCommand = new RelayCommand<object>(null, Responde);
        }

        protected void Responde(object accept)
        {
            bool accepted = accept != null && bool.Parse(accept.ToString());
            ResponseFriendRequest response = new ResponseFriendRequest();
            response.Accepted = accepted;
            response.TargetID = Info.SenderUser.ToString();
            response.NotificationId = Info.Id;
            _connection.Send(response);
            OnResponse?.Invoke(accepted);
        }

    }
}
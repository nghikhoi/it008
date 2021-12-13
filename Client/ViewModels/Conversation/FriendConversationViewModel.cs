using System.Collections.ObjectModel;
using System.Windows.Input;
using UI.Command;
using UI.Models;
using UI.Models.Message;
using UI.Network;
using UI.Network.Packets.AfterLoginRequest.Message;
using UI.Network.Packets.AfterLoginRequest.Notification;
using UI.Network.RestAPI;
using UI.Services;

namespace UI.ViewModels {
	public class FriendConversationViewModel : ConversationViewModel {

		#region Properties

		private string _userId;
		public string UserId {
			get => _userId;
			set {
				_userId = value;
				OnPropertyChanged(nameof(UserId));
			}
		}
		
		private Relationship _relationship;
		public Relationship Relationship {
			get => _relationship;
			set {
				_relationship = value;
				OnPropertyChanged(nameof(Relationship));
				OnPropertyChanged(nameof(IsNotFriend));
			}
		}

        public bool IsNotFriend {
            get => Relationship != Relationship.Friend;
        }

		#endregion

		#region Command

		public ICommand SendRequestCommand { get; private set; }

		#endregion

		protected override void Initialize(object parameter = null) {
			base.Initialize(parameter);
		}

		public FriendConversationViewModel(ChatConnection chatConnection, IAppSession appSession, IViewModelFactory factory) : base(chatConnection, appSession, factory)
        {
            SendRequestCommand = new RelayCommand<object>(null, SendRequest);
        }

        protected void SendRequest(object param = null)
        {
            FriendRequest request = new FriendRequest();
            request.TargetID = UserId;
            _connection.Send(request);
        }

		protected override void FirstSelectLoad(object parameter = null) {
			if (ConversationId.Equals("~") && !string.IsNullOrEmpty(UserId)) {
				SingleConversationFrUserID packet = new SingleConversationFrUserID {
                    UserID = UserId
                };
                DataAPI.getData<SingleConversationFrUserIDResult>(packet, result => {
					ConversationId = result.ConversationID;
				});
				return;
			}
			base.FirstSelectLoad();
		}

	}
}
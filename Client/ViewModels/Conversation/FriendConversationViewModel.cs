using System;
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
using UI.Utils;

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

        private string _fullName;
        public string FullName {
            get => FastCodeUtils.NotEmptyStrings(ConversationName) ? ConversationName : _fullName;
            set {
                _fullName = value;
                OnPropertyChanged(nameof(FullName));
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

		public FriendConversationViewModel(ChatConnection chatConnection, PacketRespondeListener listener, IAppSession appSession, IViewModelFactory factory, IModelContext model) : base(chatConnection, appSession, factory, model)
        {
            SendRequestCommand = new RelayCommand<object>(null, SendRequest);
		}

        protected void SendRequest(object param = null)
        {
            FriendRequest request = new FriendRequest();
            request.TargetID = UserId;
            _connection.Send(request);
        }

		protected override void FirstSelectLoad(object parameter = null)
        {
            Conversation = _model.GetConversationFromUserId(Guid.Parse(UserId));
			base.FirstSelectLoad();
		}

	}
}
﻿using UI.Models.Message;
using UI.Services;

namespace UI.ViewModels {
	public abstract class MessageViewModel : ViewModelBase {

		public bool IsReceivedMessage {
			get => string.CompareOrdinal(Message.SenderID, _appSession.SessionID) != 0;
		}

		private AbstractMessage _message;
		public virtual AbstractMessage Message {
			get => _message;
			set {
				_message = value;
				OnPropertyChanged(nameof(Message));
			}
		}
		
		private string _conversationId;
		public string ConversationId {
			get => _conversationId;
			set {
				_conversationId = value;
				OnPropertyChanged(nameof(ConversationId));
			}
		}

		private readonly IAppSession _appSession;

		protected MessageViewModel(IAppSession appSession) {
			_appSession = appSession;
		}
	}
}
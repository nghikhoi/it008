using System.Windows.Media;
using UI.Models.Message;
using UI.Services;

namespace UI.ViewModels {
	public abstract class MessageViewModel : ViewModelBase
    {

        public static readonly Brush IsReceivedMessageColor = new SolidColorBrush(Color.FromArgb(255, 169, 169, 169));

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

        private Brush _bubbleColor = new SolidColorBrush(Color.FromArgb(255, 0, 128, 128));
        public Brush BubbleColor {
            get => _bubbleColor;
            set {
                _bubbleColor = value;
                OnPropertyChanged(nameof(BubbleColor));
            }
        }

		private readonly IAppSession _appSession;

		protected MessageViewModel(IAppSession appSession) {
			_appSession = appSession;
		}
	}
}
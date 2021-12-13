using System.Windows.Media;
using UI.Models.Message;
using UI.Services;

namespace UI.ViewModels {
	public class TextMessageViewModel : MessageViewModel {
		public new TextMessage Message {
			get => (TextMessage) base.Message;
			set => base.Message = value;
		}

		public string Text {
			get => Message.Message;
			set {
				Message.Message = value;
				OnPropertyChanged(nameof(Text));
			}
		}

        public TextMessageViewModel(IAppSession appSession) : base(appSession) {
			
		}
	}
}
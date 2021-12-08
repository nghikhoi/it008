using UI.Models.Message;
using UI.Services;

namespace UI.ViewModels.Messages {
	public class StickerMessageViewModel : MessageViewModel {
		public new StickerMessage Message {
			get => (StickerMessage) base.Message;
			set => base.Message = value;
		}

		public StickerMessageViewModel(IAppSession appSession) : base(appSession) {
			
		}
	}
}
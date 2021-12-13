using UI.Models.Message;
using UI.Services;

namespace UI.ViewModels {
	public class StickerMessageViewModel : MessageViewModel {
		public new StickerMessage Message {
			get => (StickerMessage) base.Message;
			set => base.Message = value;
		}

        public Sticker Sticker => Message.Sticker;

		public StickerMessageViewModel(IAppSession appSession) : base(appSession) {
			
		}
	}
}
using System.Windows.Input;
using UI.Models.Message;
using UI.Services;

namespace UI.ViewModels {
	public class AttachmentMessageViewModel : MessageViewModel {
		public new AttachmentMessage Message {
			get => (AttachmentMessage) base.Message;
			set => base.Message = value;
		}

        public string FileName => Message.FileName;

        public ICommand DownloadCommand;

        public AttachmentMessageViewModel(IAppSession appSession) : base(appSession) {
			
		}


	}
}
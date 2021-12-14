using System;
using System.Windows.Input;
using Microsoft.Win32;
using UI.Command;
using UI.Models.Message;
using UI.Network.RestAPI;
using UI.Services;

namespace UI.ViewModels {
	public class AttachmentMessageViewModel : MessageViewModel {
		public new AttachmentMessage Message {
			get => (AttachmentMessage) base.Message;
			set => base.Message = value;
		}

        public string FileName => Message.FileName;

        public ICommand DownloadCommand { get; private set; }

        public AttachmentMessageViewModel(IAppSession appSession) : base(appSession)
        {
            DownloadCommand = new RelayCommand<object>(null, o => Download());
        }

        private void Download()
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "All Files (*.*)|*.*";
            dialog.CheckPathExists = true;
            dialog.FileName = FileName;
            bool? result = dialog.ShowDialog();
            if (result == true)
            {
                string path = dialog.FileName;
                FileAPI.DownloadAttachment(ConversationId, Message.FileID, path, (sender, args) => { },
                    (sender, args) => { }, error => { }, "message");
            }
        }


	}
}
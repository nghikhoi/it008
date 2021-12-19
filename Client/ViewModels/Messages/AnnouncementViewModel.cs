using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using UI.Models.Message;
using UI.Services;

namespace UI.ViewModels.Messages
{
    class AnnouncementViewModel : MessageViewModel
    {

		public new TextMessage Message
		{
			get => (TextMessage)base.Message;
			set => base.Message = value;
		}

		public string Text
		{
			get => Message.Message;
			set
			{
				Message.Message = value;
				OnPropertyChanged(nameof(Text));
			}
		}
		public AnnouncementViewModel(IAppSession appSession) : base(appSession)
		{

		}
	}
}

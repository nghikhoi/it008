using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using Emoji.Wpf;
using UI.Models.Message;
using UI.Services;

namespace UI.ViewModels {
	public class TextMessageViewModel : MessageViewModel {

        public new TextMessage Message {
			get => (TextMessage)base.Message;
			set => base.Message = value;
		}

		public string Text {
			get => Message.Message;
			set {
				Message.Message = value;
				OnPropertyChanged(nameof(Text));
			}
		}

        public bool IsFullIcon
        {
            get
            {
                string temp = Text;
                for (var i = 0; i < temp.Length; i++)
                {
                    char c = temp[i];
                    if (Char.IsLetterOrDigit(c))
                        return false;
                }

                return true;
            }
        }
		
		public TextMessageViewModel(IAppSession appSession) : base(appSession) {
			 
	}
	}
}
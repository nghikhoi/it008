using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.ViewModels.Conversation {
    public class NicknameViewModel : ViewModelBase
    {

        public string UserId { get; set; }
        public ConversationViewModel ConversationViewModel { get; set; }
        private string _nickname;
        public string Nickname
        {
            get => _nickname;
            set
            {
                _nickname = value;
                OnPropertyChanged(nameof(Nickname));
            }
        }

        public string OriginNickname { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.ViewModels.Conversation {
    public class SingleAvatarViewModel : ViewModelBase {

        private string _avatarId;
        public string AvatarId {
            get => _avatarId;
            set {
                _avatarId = value;
                OnPropertyChanged(nameof(AvatarId));
            }
        }

    }
}

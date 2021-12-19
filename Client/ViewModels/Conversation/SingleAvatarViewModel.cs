namespace UI.ViewModels {
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

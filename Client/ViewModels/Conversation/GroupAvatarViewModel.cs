namespace UI.ViewModels {
    public class GroupAvatarViewModel : ViewModelBase
    {

        private string _userOne;
        public string UserOne
        {
            get => _userOne;
            set
            {
                _userOne = value;
                OnPropertyChanged(nameof(UserOne));
            }
        }

        private string _userTwp;
        public string UserTwo {
            get => _userTwp;
            set {
                _userTwp = value;
                OnPropertyChanged(nameof(UserTwo));
            }
        }

    }
}

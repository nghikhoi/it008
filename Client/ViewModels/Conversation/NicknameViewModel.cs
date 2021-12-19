namespace UI.ViewModels {
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

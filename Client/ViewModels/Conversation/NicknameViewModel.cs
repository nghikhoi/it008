using System;
using System.Windows.Input;
using UI.Command;
using UI.Network;
using UI.Network.Packets.AfterLoginRequest.Message;

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

        public ICommand AcceptCommand { get; private set; }

        public NicknameViewModel()
        {
            AcceptCommand = new RelayCommand<object>(null, o => Accept());
        }

        public void Accept()
        {
            SetNicknamesRequest request = new SetNicknamesRequest()
            {
                ConversationID = ConversationViewModel.Conversation.ID
            };
            request.Nicknames[Guid.Parse(UserId)] = Nickname;
            ChatConnection.Instance.Session.Send(request);
            OriginNickname = Nickname;
        }
    }
}

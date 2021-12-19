using System.Collections.ObjectModel;

namespace UI.ViewModels
{
    public class GroupBubbleViewModel : ViewModelBase
    {
        public string SenderID { get; set; }

        private string _senderName;
        public string SenderName
        {
            get => _senderName;
            set
            {
                _senderName = value;
                OnPropertyChanged(nameof(SenderName));
            }
        }

        public bool IsReceive { get; set; }
        public bool IsAnnouce { get; set; }
        private ObservableCollection<MessageViewModel> _messages;
        public ObservableCollection<MessageViewModel> Messages { get => _messages; set => _messages = value; }
        public GroupBubbleViewModel()
        {
            Messages = new ObservableCollection<MessageViewModel>();
        }
    }
}

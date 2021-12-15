using System.Collections.ObjectModel;

namespace UI.ViewModels
{
    public class GroupBubbleViewModel : ViewModelBase
    {
        public string SenderID { get; set; }
        public bool IsReceive { get; set; }
        private ObservableCollection<MessageViewModel> _messages;
        public ObservableCollection<MessageViewModel> Messages { get => _messages; set => _messages = value; }
        public GroupBubbleViewModel()
        {
            Messages = new ObservableCollection<MessageViewModel>();
        }
    }
}

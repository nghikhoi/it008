using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.ViewModels.Messages
{
    public class GroupBubbleViewModel
    {
        public bool isRecieve { get; set; }
        private ObservableCollection<MessageViewModel> _messages;
        public ObservableCollection<MessageViewModel> Messages { get => _messages; set => _messages = value; }
        public GroupBubbleViewModel()
        {
            Messages = new ObservableCollection<MessageViewModel>();
        }
    }
}

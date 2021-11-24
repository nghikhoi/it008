using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UI.Annotations;

namespace UI
{
    /// <summary>
    /// Interaction logic for ChatListItemControl.xaml
    /// </summary>
    public partial class ChatListItemControl : UserControl, INotifyPropertyChanged
    {
        
        public bool Glowing { get; set; }
        public string ConversationID { get; set; }
        private string _Title;

        public string Title {
            get => _Title;
            set {
                _Title = value;
                OnPropertyChanged();
            }
        }

        private string _Subtitle;
        public string Subtitle {
            get => _Subtitle;
            set {
                _Subtitle = value;
                OnPropertyChanged();
            }
        }
        public DateTime LastActive = new DateTime();
        public ChatListItemControl()
        {
            InitializeComponent();
        }
        public bool IsFriend { get; set; }

        public event EventHandler Click;
        
        private void ClickMaskButton_OnClick(object sender, RoutedEventArgs e) {
            Click?.Invoke(sender, e);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

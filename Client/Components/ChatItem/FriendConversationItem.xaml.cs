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

namespace UI.Components {
    /// <summary>
    /// Interaction logic for FriendConversationItem.xaml
    /// </summary>
    public partial class FriendConversationItem : UserControl {
        public bool Glowing { get; set; }
        public string ConversationID { get; set; }
        private string _title;
        public string Title {
            get => _title;
            set {
                _title = value;
                OnPropertyChanged();
            }
        }
        private string _subtitle;
        public string Subtitle {
            get => _subtitle;
            set {
                _subtitle = value;
                OnPropertyChanged();
            }
        }
        private string _avatarId;
        public string AvatarId {
            get => _avatarId;
            set {
                _avatarId = value;
                OnPropertyChanged();
            }
        }
        public DateTime LastActive = new DateTime();
        public FriendConversationItem() {
            InitializeComponent();
        }
        public bool IsFriend { get; set; }

        // Create a custom routed event by first registering a RoutedEventID
        // This event uses the bubbling routing strategy
        public static readonly RoutedEvent ClickEvent = EventManager.RegisterRoutedEvent(
            nameof(Click), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(FriendConversationItem));

        // Provide CLR accessors for the event
        public event RoutedEventHandler Click {
            add => AddHandler(ClickEvent, value);
            remove => RemoveHandler(ClickEvent, value);
        }

        private void ClickMaskButton_OnClick(object sender, RoutedEventArgs e) {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(ClickEvent);
            RaiseEvent(newEventArgs);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

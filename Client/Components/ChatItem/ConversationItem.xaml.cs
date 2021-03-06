using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using UI.Annotations;

namespace UI.Components
{
    /// <summary>
    /// Interaction logic for ChatListItemControl.xaml
    /// </summary>
    public partial class ConversationItem : UserControl, INotifyPropertyChanged
    {
        
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
        public ConversationItem()
        {
            InitializeComponent();
        }
        public bool IsFriend { get; set; }
        
        // Create a custom routed event by first registering a RoutedEventID
        // This event uses the bubbling routing strategy
        public static readonly RoutedEvent ClickEvent = EventManager.RegisterRoutedEvent(
            nameof(Click), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ConversationItem));

        // Provide CLR accessors for the event
        public event RoutedEventHandler Click
        {
            add { AddHandler(ClickEvent, value); }
            remove { RemoveHandler(ClickEvent, value); }
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

using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace UI.Components
{
    /// <summary>
    /// Interaction logic for ucListRecentMessage.xaml
    /// </summary>
    public partial class ConversationListTabs : UserControl {
        private Border[] tabMarks = new Border[2];
        public string UserID {
            get;
            set;
        }
        
        public Button AvatarButton {
            get => this.TogglePopupButton;
            set {
                this.TogglePopupButton = value;
                SetValue(AvatarButtonProperty, value);
            }
        }
        public static readonly DependencyProperty AvatarButtonProperty = DependencyProperty.Register(nameof(AvatarButton), typeof (IEnumerable), typeof (ConversationListTabs), new FrameworkPropertyMetadata((object) null));
        public static readonly DependencyProperty ConversationListProperty = DependencyProperty.Register(nameof (ConversationList), typeof (IEnumerable), typeof (ConversationListTabs), new FrameworkPropertyMetadata((object) null));
        public IEnumerable ConversationList
        {
            get => (IEnumerable) GetValue(ConversationListProperty);
            set
            {
                if (value == null)
                    this.ClearValue(ConversationListProperty);
                else
                    this.SetValue(ConversationListProperty, value);
            }
        }
        
        public static readonly DependencyProperty SearchingListProperty = DependencyProperty.Register(nameof (SearchingList), typeof (IEnumerable), typeof (ConversationListTabs), new FrameworkPropertyMetadata((object) null));
        public IEnumerable SearchingList
        {
            get => (IEnumerable) GetValue(SearchingListProperty);
            set
            {
                if (value == null)
                    this.ClearValue(SearchingListProperty);
                else
                    this.SetValue(SearchingListProperty, value);
            }
        }
        
        public static readonly RoutedEvent AvatarCheckedEvent = EventManager.RegisterRoutedEvent("AvatarCheckedEvent", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ConversationListTabs));
        public static readonly RoutedEvent AvatarUncheckedEvent = EventManager.RegisterRoutedEvent("AvatarUncheckedEvent", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ConversationListTabs));
        
        // Create a custom routed event by first registering a RoutedEventID
        // This event uses the bubbling routing strategy
        public static readonly RoutedEvent AvatarClickEvent = EventManager.RegisterRoutedEvent(
            nameof(AvatarClick), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ConversationListTabs));

        // Provide CLR accessors for the event
        public event RoutedEventHandler AvatarClick
        {
            add { AddHandler(AvatarClickEvent, value); }
            remove { RemoveHandler(AvatarClickEvent, value); }
        }

        // This method raises the Tap event
        void RaiseTapEvent()
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(AvatarClickEvent);
            RaiseEvent(newEventArgs);
        }
        // For demonstration purposes we raise the event when the MyButtonSimple is clicked
        protected void OnAvatarClick(object sender, RoutedEventArgs args) {
            RaiseTapEvent();
        }
        
        public event RoutedEventHandler AvatarChecked {
            add {
                //this.TogglePopupButton.AddHandler(ToggleButton.CheckedEvent, value);
                this.AddHandler(AvatarCheckedEvent, value);
            }
            remove {
                //this.TogglePopupButton.RemoveHandler(ToggleButton.CheckedEvent, value);
                this.RemoveHandler(AvatarCheckedEvent, value);
            }
        }

        public event RoutedEventHandler AvatarUnchecked {
            add {
                //this.TogglePopupButton.AddHandler(ToggleButton.UncheckedEvent, value);
                this.AddHandler(AvatarUncheckedEvent, value);
            }
            remove {
                //this.TogglePopupButton.RemoveHandler(ToggleButton.UncheckedEvent, value);
                this.RemoveHandler(AvatarUncheckedEvent, value);
            }
        }

        public ConversationListTabs()
        {
            InitializeComponent();
            transitioner.SelectedIndex = 1;
            tabMarks[0] = RecentTabSelectMark;
            tabMarks[1] = ContactsRabSelectMark;
            selectTab(1);
        }

        #region RecentMsg & FriendList
        //ROW 1
        private void selectTab(int index)
        {
            foreach (var mark in tabMarks) mark.Visibility = Visibility.Hidden;
            tabMarks[index].Visibility = Visibility.Visible;
        }

        private void btnRecent_Click(object sender, RoutedEventArgs e)
        {
            transitioner.SelectedIndex = 0;
            selectTab(0);
        }

        private void btnListFriend_Click(object sender, RoutedEventArgs e)
        {
            transitioner.SelectedIndex = 1;
            selectTab(1);
        }


        public void update_friend_list(object ob)
        {
            if(ob is ConversationItem)
            {
                this.listFriend.Children.Add(ob as ConversationItem);
            }
        }

        public void update_recent_conversation(object ob)
        {
            if (ob is ConversationItem)
            {
                // this.RecentTab.Container.Children.Add(ob as ChatListItemControl);
            }
        }

        public void clear_friend_list()
        {
            this.listFriend.Children.Clear();
        }

        public void clear_recent_conversation()
        {
            // this.RecentTab.Container.Children.Clear();
        }
        private void SearchAction(object sender, TextChangedEventArgs e)
        {
        }
        #endregion

        private void TogglePopupButton_Checked(object sender, RoutedEventArgs e)
        {
            // ChatWindow chatWindow = ModuleContainer.GetModule<ChatWindow>();
            // chatWindow.view.ChatPage.Fade.Visibility = Visibility.Visible;
        }

        private void TogglePopupButton_UnChecked(object sender, RoutedEventArgs e)
        {
            // ChatWindow chatWindow = ModuleContainer.GetModule<ChatWindow>();
            // chatWindow.view.ChatPage.Fade.Visibility = Visibility.Hidden;
        }

        private void NotificationPage_Loaded(object sender, RoutedEventArgs e)
        {

        }
        private void InfOpenBtn_Click(object sender, RoutedEventArgs e)
        {
        }

        private void LogOutBtn_Click(object sender, RoutedEventArgs e) {
        }

        //private void newMessage_Click(object sender, RoutedEventArgs e)
        //{
        //    NewConversationWindow newcon = new NewConversationWindow();
        //    newcon.ShowDialog();
        //}
    }

}

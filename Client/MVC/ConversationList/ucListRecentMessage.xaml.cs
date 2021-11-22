using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using UI.MVC;

namespace UI
{
    /// <summary>
    /// Interaction logic for ucListRecentMessage.xaml
    /// </summary>
    public partial class ucListRecentMessage : UserControl, IView {
        private Border[] tabMarks = new Border[2];
        public string UserID {
            get => ChatModel.Instance.SelfID;
            set {}
        }
        
        public ToggleButton AvatarButton {
            get => this.TogglePopupButton;
            set => this.TogglePopupButton = value;
        }

        public static readonly RoutedEvent AvatarCheckedEvent = EventManager.RegisterRoutedEvent("AvatarCheckedEvent", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ucListRecentMessage));
        public static readonly RoutedEvent AvatarUncheckedEvent = EventManager.RegisterRoutedEvent("AvatarUncheckedEvent", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ucListRecentMessage));
        
        public event RoutedEventHandler AvatarChecked {
            add {
                this.TogglePopupButton.AddHandler(ToggleButton.CheckedEvent, value);
                this.AddHandler(AvatarCheckedEvent, value);
            }
            remove {
                this.TogglePopupButton.RemoveHandler(ToggleButton.CheckedEvent, value);
                this.RemoveHandler(AvatarCheckedEvent, value);
            }
        }

        public event RoutedEventHandler AvatarUnchecked {
            add {
                this.TogglePopupButton.AddHandler(ToggleButton.UncheckedEvent, value);
                this.AddHandler(AvatarUncheckedEvent, value);
            }
            remove {
                this.TogglePopupButton.RemoveHandler(ToggleButton.UncheckedEvent, value);
                this.RemoveHandler(AvatarUncheckedEvent, value);
            }
        }

        public ucListRecentMessage()
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
            if(ob is ChatListItemControl)
            {
                this.listFriend.Children.Add(ob as ChatListItemControl);
            }
        }

        public void update_recent_conversation(object ob)
        {
            if (ob is ChatListItemControl)
            {
                this.RecentTab.Container.Children.Add(ob as ChatListItemControl);
            }
        }

        public void clear_friend_list()
        {
            this.listFriend.Children.Clear();
        }

        public void clear_recent_conversation()
        {
            this.RecentTab.Container.Children.Clear();
        }
        private void SearchAction(object sender, TextChangedEventArgs e)
        {
            ConversationListController controller = ModuleContainer.GetModule<ConversationList>().controller;
            if (searchInput.Text == "") {
                controller.loadFriends();
            } else controller.SearchAction(searchInput.Text);
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
            var parentwin = Window.GetWindow(this) as HomeWindow;
            parentwin.make_fade();

            SettingPage settingPage = ModuleContainer.GetModule<SettingPage>();
            settingPage.view.Show();
        }

        private void LogOutBtn_Click(object sender, RoutedEventArgs e) {
            ChatWindow chatWindow = ModuleContainer.GetModule<ChatWindow>();
            chatWindow.controller.LogOut();
        }

        //private void newMessage_Click(object sender, RoutedEventArgs e)
        //{
        //    NewConversationWindow newcon = new NewConversationWindow();
        //    newcon.ShowDialog();
        //}
    }
}

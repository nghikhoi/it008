using System;
using System.Collections.Generic;
using System.Linq;
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
using UI.MVC;

namespace UI
{
    /// <summary>
    /// Interaction logic for ucListRecentMessage.xaml
    /// </summary>
    public partial class ucListRecentMessage : UserControl, IView
    {
        private Border[] tabMarks = new Border[2];

        public ucListRecentMessage()
        {
            InitializeComponent();
            transitioner.SelectedIndex = 1;
            tabMarks[0] = RecentTabSelectMark;
            tabMarks[1] = ContactsRabSelectMark;
            selectTab(1);
        }

        //private void accInfo_Btn_Click(object sender, RoutedEventArgs e)
        //{
        //    SettingWindow settingWindow = new SettingWindow();
        //    myPopup.IsOpen = false;
        //    var parentwin = Window.GetWindow(this) as HomeWindow;
        //    parentwin.make_fade();
        //    settingWindow.Show();
        //}

        //private void logOutBtn_Click(object sender, RoutedEventArgs e)
        //{
        //    WindowLogIn windowLogIn = new WindowLogIn();
        //    myPopup.IsOpen = false;
        //    Window window = Window.GetWindow(this);
        //    if (window != null)
        //        window.Close();
        //    windowLogIn.ShowDialog();
        //}


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
            //ConversationListController controller = ModuleContainer.GetModule<ConversationList>().controller;
            //if (searchInput.Text == "")
            //{
                
            //}
            //else
            //    controller.SearchAction(searchInput.Text);
        }
        
        private void TogglePopupButton_Checked(object sender, RoutedEventArgs e) {
            // ChatWindow chatWindow = ModuleContainer.GetModule<ChatWindow>();
            // chatWindow.view.FullFade.Visibility = Visibility.Visible;
        }

        private void TogglePopupButton_UnChecked(object sender, RoutedEventArgs e)
        {
            // ChatWindow chatWindow = ModuleContainer.GetModule<ChatWindow>();
            // chatWindow.view.FullFade.Visibility = Visibility.Hidden;
        }

        private void NotificationPage_Loaded(object sender, RoutedEventArgs e)
        {

        }
        private void InfOpenBtn_Click(object sender, RoutedEventArgs e)
        {
            SettingWindow settingWindow = new SettingWindow();
            var parentwin = Window.GetWindow(this) as HomeWindow;
            parentwin.make_fade();
            settingWindow.Show();
        }

        private void LogOutBtn_Click(object sender, RoutedEventArgs e)
        {
            WindowLogIn windowLogIn = new WindowLogIn();
            Window window = Window.GetWindow(this);
            if (window != null)
                window.Close();
            windowLogIn.Show();
        }




        //private void newMessage_Click(object sender, RoutedEventArgs e)
        //{
        //    NewConversationWindow newcon = new NewConversationWindow();
        //    newcon.ShowDialog();
        //}
    }
}

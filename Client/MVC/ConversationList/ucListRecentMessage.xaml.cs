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

        private void accInfo_Btn_Click(object sender, RoutedEventArgs e)
        {
            SettingWindow settingWindow = new SettingWindow();
            myPopup.IsOpen = false;
            var parentwin = Window.GetWindow(this) as HomeWindow;
            parentwin.make_fade();
            settingWindow.Show();
        }

        private void logOutBtn_Click(object sender, RoutedEventArgs e)
        {
            WindowLogIn windowLogIn = new WindowLogIn();
            myPopup.IsOpen = false;
            Window window = Window.GetWindow(this);
            if (window != null)
                window.Close();
            windowLogIn.ShowDialog();
        }

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

        private void LoadConverstionList(object sender, RoutedEventArgs e)
        {
            ConversationListController controller = ModuleContainer.GetModule<ConversationList>().controller;
            controller.AddShortInfoConversation_active();
        }

        public void update_conversation_list(object ob)
        {
            if(ob is ChatListItemControl)
            {
                this.listFriend.Children.Add(ob as ChatListItemControl);
            }
        }

        public void clear_friend_list()
        {
            this.listFriend.Children.Clear();
        }
        private void SearchAction(object sender, TextChangedEventArgs e)
        {
            ConversationListController controller = ModuleContainer.GetModule<ConversationList>().controller;
            if (searchInput.Text == "")
            {
                controller.AddShortInfoConversation_active();
            }
        }
        //private void newMessage_Click(object sender, RoutedEventArgs e)
        //{
        //    NewConversationWindow newcon = new NewConversationWindow();
        //    newcon.ShowDialog();
        //}
    }
}

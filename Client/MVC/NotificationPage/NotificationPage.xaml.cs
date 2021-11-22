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
    /// Interaction logic for NotificationPage.xaml
    /// </summary>
    public partial class NotificationPage : UserControl, IView
    {
        public NotificationPage()
        {
            InitializeComponent();
        }

        //HEADER
        private void InfOpenBtn_Click(object sender, RoutedEventArgs e)
        {
            var parentwin = Window.GetWindow(this) as HomeWindow;
            parentwin.make_fade();
            SettingPage settingPage = ModuleContainer.GetModule<SettingPage>();
            settingPage.view.Show();
        }

        //BODY

        //END
        private void LogOutBtn_Click(object sender, RoutedEventArgs e)
        {
            ChatWindow chatWindow = ModuleContainer.GetModule<ChatWindow>();
            chatWindow.controller.LogOut();
        }

    }
}

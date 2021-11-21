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

namespace UI
{
    /// <summary>
    /// Interaction logic for NotificationPage.xaml
    /// </summary>
    public partial class NotificationPage : UserControl
    {
        public NotificationPage()
        {
            InitializeComponent();
        }

        //HEADER
        private void InfOpenBtn_Click(object sender, RoutedEventArgs e)
        {
            SettingWindow settingWindow = new SettingWindow();
            var parentwin = Window.GetWindow(this) as HomeWindow;
            parentwin.make_fade();
            settingWindow.Show();
        }

        //BODY

        //END
        private void LogOutBtn_Click(object sender, RoutedEventArgs e)
        {
            WindowLogIn windowLogIn = new WindowLogIn();
            Window window = Window.GetWindow(this);
            if (window != null)
                window.Close();
            windowLogIn.Show();
        }

    }
}

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
    /// Interaction logic for ucListRecentMessage.xaml
    /// </summary>
    public partial class ucListRecentMessage : UserControl
    {
        public ucListRecentMessage()
        {
            InitializeComponent();
        }

        private void accInfo_Btn_Click(object sender, RoutedEventArgs e)
        {
            SettingWindow settingWindow = new SettingWindow();
            myPopup.IsOpen = false;
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
    }
}

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
using System.Windows.Shapes;
using UI.Models;
using Microsoft.Win32;
using UI.MVC;

namespace UI
{
    /// <summary>
    /// Interaction logic for HomeWindow.xaml
    /// </summary>
    public partial class HomeWindow : Window, IView
    {
        public HomeWindow()
        {
            InitializeComponent();
        }
        private void DockPanel_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            this.DragMove();
        }

        private void Image_PreviewMouseDown_close(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void Image_PreviewMouseDown_minimize(object sender, MouseButtonEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
        private void accInfo_Btn_Click(object sender, RoutedEventArgs e)
        {
            SettingWindow settingWindow = new SettingWindow();
            myPopup.IsOpen = false;
            settingWindow.Show();
        }


        
        private void ucTitleBar_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void logOutBtn_Click(object sender, RoutedEventArgs e)
        {
            WindowLogIn windowLogIn = new WindowLogIn();
            myPopup.IsOpen = false;
            this.Close();
            windowLogIn.ShowDialog();
        }
    }
    public class WindowChrome : Freezable
    {
        protected override Freezable CreateInstanceCore()
        {
            throw new System.NotImplementedException();
        }
    }

}

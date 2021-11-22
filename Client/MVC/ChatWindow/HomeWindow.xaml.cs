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
using System.Windows.Media.Animation;

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
        
        private void ucTitleBar_Loaded(object sender, RoutedEventArgs e)
        {

        }

        public void make_fade()
        {
            this.Fade.Visibility = Visibility.Visible;
        }

        public void make_clear()
        {
            this.Fade.Visibility = Visibility.Hidden;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            foreach (Window win in Application.Current.Windows) {
                if (win is IView)
                    win.Hide();
                else win.Close();
            }
        }

        #region Gallery
        //Transition Gallery
        public void OpenProfileDisplayer()
        {
            //ProfileDisplayer.Display(id, name, email, dob, address);
            var sb = this.FindResource("left-side-panel-expand") as Storyboard;
            sb.Begin();
        }

        public void CloseProfileDisplayer()
        {
            var sb = this.FindResource("left-side-panel-compress") as Storyboard;
            sb.Begin();
        }
        #endregion
    }
    //public class WindowChrome : Freezable
    //{
    //    protected override Freezable CreateInstanceCore()
    //    {
    //        throw new System.NotImplementedException();
    //    }
    //}

}

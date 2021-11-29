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
            //ucTitleBar1.btnFullScreen.Click += btnFullScreen_Click;
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

        #region NotificationPage Toggle

        private bool IsNotificationVisible {
            get => NotificaionPage.ActualWidth > 0;
        }

        public void TurnOnNotificationPage() {
            if (IsNotificationVisible) return;
            DoubleAnimation open = new DoubleAnimation();
            open.From = 0;
            open.To = MessageList.ActualWidth;
            open.Duration = new Duration(TimeSpan.Parse("0:0:0.2"));
            NotificaionPage.BeginAnimation(NotificationPage.WidthProperty, open);
            TurnOnFade();
        }

        public void TurnOffNotificationPage() {
            if (!IsNotificationVisible) return;
            DoubleAnimation close = new DoubleAnimation();
            close.From = NotificaionPage.ActualWidth;
            close.To = 0;
            close.Duration = new Duration(TimeSpan.Parse("0:0:0.2"));
            NotificaionPage.BeginAnimation(NotificationPage.WidthProperty, close);
            TurnOffFade();
        }

        public void ToggleNotificationPage() {
            if (!IsNotificationVisible) {
                TurnOnNotificationPage();
            }
            else {
                TurnOffNotificationPage();
            }
        }

        #endregion

        #region Fade Toggle

        private bool IsFadeVisible {
            get => Fade.Opacity > 0;
        }

        public void TurnOnFade() {
            if (IsNotificationVisible) return;
            Fade.Visibility = Visibility.Visible;
            DoubleAnimation open = new DoubleAnimation();
            open.From = 0;
            open.To = 0.5;
            open.Duration = new Duration(TimeSpan.Parse("0:0:0.2"));
            Fade.BeginAnimation(Button.OpacityProperty, open);
        }

        public void TurnOffFade() {
            if (!IsNotificationVisible) return;
            DoubleAnimation close = new DoubleAnimation();
            close.From = Fade.Opacity;
            close.To = 0;
            close.Duration = new Duration(TimeSpan.Parse("0:0:0.2"));
            close.Completed += (s, e) => {
                Fade.Visibility = Visibility.Hidden;
            };
            Fade.BeginAnimation(Button.OpacityProperty, close);
        }

        public void ToggleFade() {
            if (!IsFadeVisible) {
                TurnOnFade();
            }
            else {
                TurnOffFade();
            }
        }

        #endregion
        
        private void MessageList_OnAvatarChecked(object sender, RoutedEventArgs e) {
            ToggleNotificationPage();
        }

        private void MessageList_OnAvatarUnchecked(object sender, RoutedEventArgs e) {
            ToggleNotificationPage();
        }

        private void Fade_OnClick(object sender, RoutedEventArgs e) {
            TurnOffNotificationPage();
        }

        private void NotificaionPage_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void ucTitleBar1_Loaded(object sender, RoutedEventArgs e)
        {

        }

        //private void btnFullScreen_Click(object sender, RoutedEventArgs e)
        //{
        //    isMaximized = !isMaximized;
        //}

        //private bool _isMaximized;
        //public bool isMaximized
        //{
        //    get
        //    {
        //        return _isMaximized;
        //    }
        //    set
        //    {
        //        _isMaximized = value;
        //        this.WindowState = _isMaximized == false ? WindowState.Normal : WindowState.Maximized;
        //    }
        //}
    }
    //public class WindowChrome : Freezable
    //{
    //    protected override Freezable CreateInstanceCore()
    //    {
    //        throw new System.NotImplementedException();
    //    }
    //}

}

using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Threading;
using UI.Components;
using UI.MVC;


namespace UI
{
    /// <summary>
    /// Interaction logic for SettingWindow.xaml
    /// </summary>
    public partial class SettingWindow : Window, IView
    {
        public SettingWindow()
        {
            InitializeComponent();
            DataContext = new ChangeUserInfomation();
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

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            // nơi thực hiện các nhiệm vụ background
        }

        private void ChangeNameBtn_Click(object sender, RoutedEventArgs e)
        {
            ChangeName changeName = new ChangeName();
            FullFade.Visibility = Visibility.Visible;
            changeName.ShowDialog();
            if (!changeName.IsActive)
            {
                FullFade.Visibility = Visibility.Hidden;
            }

        }

        private void ChangePassWord_btn_Click_1(object sender, RoutedEventArgs e)
        {
            ChangePassWord changePassWord = new ChangePassWord();
            ////Lock backgroundWindow
            //changePassWord.Owner = this;
            FullFade.Visibility = Visibility.Visible;
            changePassWord.ShowDialog();
            if (!changePassWord.IsActive)
            {
                FullFade.Visibility = Visibility.Hidden;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

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
namespace UI
{
    /// <summary>
    /// Interaction logic for HomeWindow.xaml
    /// </summary>
    public partial class HomeWindow : Window
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
        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            HomeWindow home = new HomeWindow();
            home.Show();
            this.Close();
        }

        private void SignBtn_Click(object sender, RoutedEventArgs e)
        {
            WindowRegister home = new WindowRegister();
            home.Show();
            this.Close();
        }

        private void accInfo_Btn_Click(object sender, RoutedEventArgs e)
        {
            SettingWindow settingWindow = new SettingWindow();
            settingWindow.Show();
            
        }

        private void update_message_container(message tmp)
        {
            if(tmp is text_message) {
                text_message offi = (text_message)tmp;
                var msg = new ucChatItem();
                msg.text_msg_content.Text = offi.content;
                msg.HorizontalAlignment = HorizontalAlignment.Right;
                msg.VerticalAlignment = VerticalAlignment.Bottom;
                message_container.Children.Add(msg);
                msg_scroll.ScrollToEnd();
            }
            
        }
        private void update_meaage_container_rcv(message tmp)
        {
            if (tmp is text_message)
            {
                text_message offi = (text_message)tmp;
                var msg = new ucChatItem();
                msg.message_border.Background = Brushes.Gray;
                msg.text_msg_content.Text = offi.content;
                msg.HorizontalAlignment = HorizontalAlignment.Left;
                msg.VerticalAlignment = VerticalAlignment.Bottom;
                message_container.Children.Add(msg);
                msg_scroll.ScrollToEnd();
            }
        }
        private void send_on_click(object sender, RoutedEventArgs e)
        {
            if (ChatInput.Text != "")
            {
                text_message tmp = new text_message(ChatInput.Text);
                
                update_message_container(tmp);
               
                update_meaage_container_rcv(new text_message("test recieve."));
                ChatInput.Text = "";
            }
        }


        private void send_by_enter(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if(Keyboard.IsKeyDown(Key.LeftShift)|| Keyboard.IsKeyDown(Key.RightShift)){
                    ChatInput.Text += "\n";
                 
                    return;
                }
                if (ChatInput.Text != "")
                {
                    text_message tmp = new text_message(ChatInput.Text);

                    update_message_container(tmp);
                    ChatInput.Text = "";
                }
            }
        }

        private void ucTitleBar_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void update_image_message(BitmapImage image)
        {
            Image msgimage = new Image();
            msgimage.Source = image;
            msgimage.MaxWidth = 300;
            msgimage.HorizontalAlignment = HorizontalAlignment.Right;
            message_container.Children.Add(msgimage);
            msg_scroll.ScrollToEnd();
        }

        private void send_image_on_click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog opendlg = new OpenFileDialog();
            opendlg.Filter = "Image files (*.jpg, *.png) |*.jpg;*.png";
            if (opendlg.ShowDialog() == true)
            {
                Uri fileuri = new Uri(opendlg.FileName);
                BitmapImage myimage = new BitmapImage();
                myimage.BeginInit();
                myimage.UriSource = fileuri;
                myimage.EndInit();
                update_image_message(myimage);
            }
        }

        private void update_file_message(string fileurl)
        {
            //var mebutton = new mediaPlayer();
            //mebutton.video_link.Text = fileurl;
            //mebutton.HorizontalAlignment = HorizontalAlignment.Right;
            //message_container.Children.Add(mebutton);
            var videomsg = new VideoMessage();
            Uri videopath = new Uri(fileurl);
            videomsg.VideoControl.Source = videopath;
            videomsg.MaxWidth = 300;
            videomsg.HorizontalAlignment = HorizontalAlignment.Right;
            message_container.Children.Add(videomsg);
            msg_scroll.ScrollToEnd();
        }
        private void send_files_on_click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog opendlg = new OpenFileDialog();
            opendlg.Filter = "Video files (*.MP4) |*.MP4";
            if (opendlg.ShowDialog() == true) 
            {
                update_file_message(opendlg.FileName);
            }
            
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

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
    }
    public class WindowChrome : Freezable
    {
        protected override Freezable CreateInstanceCore()
        {
            throw new System.NotImplementedException();
        }
    }

}

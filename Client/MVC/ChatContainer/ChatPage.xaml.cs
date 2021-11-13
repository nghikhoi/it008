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
using UI.Models;
using Microsoft.Win32;
using UI.Models.Message;
using UI.MVC;

namespace UI
{
    /// <summary>
    /// Interaction logic for ChatPage.xaml
    /// </summary>
    public partial class ChatPage : UserControl, IView {

        private static ChatPage instance;

        public static ChatPage Instance {
            get => instance == null ? (instance = new ChatPage()) : instance;
        }
        
        private ChatContainerController controller;
        
        public ChatPage()
        {
            InitializeComponent();
            
            controller = new ChatContainerController(this);
            ChatContainer module = new ChatContainer();
            module.InitializeMVC(ChatModel.Instance, this, controller);
        }
        private void update_message_container(AbstractMessage tmp)
        {
            if (tmp is TextMessage)
            {
                TextMessage offi = (TextMessage) tmp;
                var msg = new ucChatItem();
                msg.text_msg_content.Text = offi.Message;
                msg.HorizontalAlignment = HorizontalAlignment.Right;
                msg.VerticalAlignment = VerticalAlignment.Bottom;
                message_container.Children.Add(msg);
                msg_scroll.ScrollToEnd();
            }

        }
        
        private void update_meaage_container_rcv(AbstractMessage tmp)
        {
            if (tmp is TextMessage)
            {
                TextMessage offi = (TextMessage)tmp;
                var msg = new Components.ucChatItemSender();
                msg.message_border.Background = Brushes.Gray;
                msg.text_msg_content.Text = offi.Message;
                msg.HorizontalAlignment = HorizontalAlignment.Left;
                msg.VerticalAlignment = VerticalAlignment.Bottom;
                message_container.Children.Add(msg);
                msg_scroll.ScrollToEnd();
            }
        }
        
        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            if (ChatInput.Text != "")
            {
                TextMessage tmp = new TextMessage();
                tmp.Message = ChatInput.Text;

                update_message_container(tmp);

                update_meaage_container_rcv(new TextMessage("test recieve."));
                ChatInput.Text = "";
            }
        }


        private void send_by_enter(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
                {
                    ChatInput.Text += "\n";

                    return;
                }
                if (ChatInput.Text != "")
                {
                    TextMessage tmp = new TextMessage(ChatInput.Text);

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
            var msgimage = new ImageMsg();
            msgimage.ImageControl.Source = image;
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
            var videomsg = new VideoMessage();
            Uri videopath = new Uri(fileurl);
            var tl = new MediaTimeline(videopath);
            videomsg.VideoControl.Source = videopath;
            videomsg.VideoControl.Clock = tl.CreateClock(true) as MediaClock;
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
}

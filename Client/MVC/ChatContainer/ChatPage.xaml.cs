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

        #region TextMessage function
        public void update_message_container(AbstractMessage tmp)
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
<<<<<<< HEAD

=======
>>>>>>> ef7ec2edc40dbc5cabe5e57d94ea2bde2617f11d
        public void update_meaage_container_rcv(AbstractMessage tmp)
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

        public void update_msgcontainer_at_top(AbstractMessage tmp)
        {
            if (tmp is TextMessage)
            {
                TextMessage offi = (TextMessage)tmp;
                var msg = new ucChatItem();
                msg.text_msg_content.Text = offi.Message;
                msg.HorizontalAlignment = HorizontalAlignment.Right;
                msg.VerticalAlignment = VerticalAlignment.Top;
                spc_chat_container.Children.Add(msg);
            }
        }

        public void update_msgcontainer_rcv_at_top(AbstractMessage tmp)
        {
            if (tmp is TextMessage)
            {
                TextMessage offi = (TextMessage)tmp;
                var msg = new ucChatItem();
                msg.text_msg_content.Text = offi.Message;
                msg.HorizontalAlignment = HorizontalAlignment.Left;
                msg.VerticalAlignment = VerticalAlignment.Top;
                spc_chat_container.Children.Add(msg);
            }
        }
        #endregion
        public void btnSend_Click(object sender, RoutedEventArgs e)
        {
            if (ChatInput.Text != "")
            {
                TextMessage tmp = new TextMessage();
                tmp.Message = ChatInput.Text;

                update_msgcontainer_at_top(tmp);
                ChatInput.Text = "";
            }
        }


        public void send_by_enter(object sender, KeyEventArgs e)
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

        #region Image function
        public void update_image_message(BitmapImage image)
        {
            var msgimage = new ImageMsg();
            msgimage.ImageControl.Source = image;
            msgimage.MaxWidth = 300;
            msgimage.HorizontalAlignment = HorizontalAlignment.Right;
            message_container.Children.Add(msgimage);
            msg_scroll.ScrollToEnd();
        }

        public void update_image_message_rcv(BitmapImage image)
        {
            var msgimage = new ImageMsg();
            msgimage.ImageControl.Source = image;
            msgimage.MaxWidth = 300;
            msgimage.HorizontalAlignment = HorizontalAlignment.Left;
            message_container.Children.Add(msgimage);
            msg_scroll.ScrollToEnd();
        }

        public void update_image_message_at_top(BitmapImage image)
        {
            var msgimage = new ImageMsg();
            msgimage.ImageControl.Source = image;
            msgimage.MaxWidth = 300;
            msgimage.HorizontalAlignment = HorizontalAlignment.Right;
            spc_chat_container.Children.Add(msgimage);
        }

        public void update_image_message_rcv_at_top(BitmapImage image)
        {
            var msgimage = new ImageMsg();
            msgimage.ImageControl.Source = image;
            msgimage.MaxWidth = 300;
            msgimage.HorizontalAlignment = HorizontalAlignment.Left;
            spc_chat_container.Children.Add(msgimage);
        }
        #endregion
        public void send_image_on_click(object sender, RoutedEventArgs e)
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

        #region media
        public void update_file_message(Uri videopath)
        {
            var videomsg = new SimpleMediaPlayer();
            var tl = new MediaTimeline(videopath);
            videomsg.VideoControl.Source = videopath;
            videomsg.VideoControl.Clock = tl.CreateClock(true) as MediaClock;
            videomsg.MaxWidth = 300;
            videomsg.HorizontalAlignment = HorizontalAlignment.Right;
            message_container.Children.Add(videomsg);
            msg_scroll.ScrollToEnd();
        }

        public void update_file_message_rcv(Uri videopath)
        {
            var videomsg = new SimpleMediaPlayer();
            var tl = new MediaTimeline(videopath);
            videomsg.VideoControl.Source = videopath;
            videomsg.VideoControl.Clock = tl.CreateClock(true) as MediaClock;
            videomsg.MaxWidth = 300;
            videomsg.HorizontalAlignment = HorizontalAlignment.Left;
            message_container.Children.Add(videomsg);
            msg_scroll.ScrollToEnd();
        }

        public void update_file_message_at_top(Uri videopath)
        {
            var videomsg = new SimpleMediaPlayer();
            var tl = new MediaTimeline(videopath);
            videomsg.VideoControl.Source = videopath;
            videomsg.VideoControl.Clock = tl.CreateClock(true) as MediaClock;
            videomsg.MaxWidth = 300;
            videomsg.HorizontalAlignment = HorizontalAlignment.Right;
            spc_chat_container.Children.Add(videomsg);
        }

        public void update_file_message_rcv_at_top(Uri videopath)
        {
            var videomsg = new SimpleMediaPlayer();
            var tl = new MediaTimeline(videopath);
            videomsg.VideoControl.Source = videopath;
            videomsg.VideoControl.Clock = tl.CreateClock(true) as MediaClock;
            videomsg.MaxWidth = 300;
            videomsg.HorizontalAlignment = HorizontalAlignment.Left;
            spc_chat_container.Children.Add(videomsg);
        }

        #endregion
        private void send_files_on_click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog opendlg = new OpenFileDialog();
            opendlg.Filter = "Video files (*.MP4) |*.MP4";
            if (opendlg.ShowDialog() == true)
            {
                Uri filepath = new Uri(opendlg.FileName);
                update_file_message(filepath);
            }

        }

        

        private void detect_at_top(object sender, ScrollChangedEventArgs e)
        {
            if (msg_scroll.Height != 0)
            {
                if (msg_scroll.VerticalOffset == 0) { }
                   //todo: do something here
            }
        }
        private void LoadMessagesBtn_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}

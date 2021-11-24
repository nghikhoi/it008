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
using System.Windows.Media.Animation;
using UI.Components;

namespace UI
{
    /// <summary>
    /// Interaction logic for ChatPage.xaml
    /// </summary>
    public partial class ChatPage : UserControl, IView {

        public delegate void FormatBubble(UserControl userControl);
        
        private ChatContainer module {
            get => ModuleContainer.GetModule<ChatContainer>();
        }
        public int Top { get; private set; }
        public int Left { get; private set; }

        public event FormatBubble LeftBubbleFormat = uc => {
            uc.HorizontalAlignment = HorizontalAlignment.Left;
            uc.VerticalAlignment = VerticalAlignment.Bottom;
        };
        public event FormatBubble RightBubbleFormat = uc => {
            uc.HorizontalAlignment = HorizontalAlignment.Right;
            uc.VerticalAlignment = VerticalAlignment.Bottom;
        };
        
        public ChatPage()
        {
            InitializeComponent();
        }

        public void cleanChatPage() {
            message_container.Children.Clear();
        }

        #region AddMessage

        #region TextMessage
        public void AddOwnedMessage(TextMessage tmp, bool toTop = false)
        {
            var msg = new ucChatItem();
            msg.text_msg_content.Text = tmp.Message;
            RightBubbleFormat?.Invoke(msg);
            addMessage(msg, toTop);
            msg_scroll.ScrollToEnd();
        }

        public void AddReceiveMessage(TextMessage tmp, bool toTop = false) {
            var msg = new Components.ucChatItemSender();
            msg.message_border.Background = Brushes.Gray;
            msg.text_msg_content.Text = tmp.Message;
            LeftBubbleFormat?.Invoke(msg);
            addMessage(msg, toTop);
            msg_scroll.ScrollToEnd();
        }
        #endregion

        #region MediaMessage
        public void AddOwnedMessage(MediaInfo info, bool toTop = false)
        {
            var videomsg = new ThumbnailBubble(info);
            videomsg.MaxWidth = 300;
            RightBubbleFormat?.Invoke(videomsg);
            addMessage(videomsg, toTop);
            msg_scroll.ScrollToEnd();
        }

        public void AddReceiveMessage(MediaInfo info, bool toTop = false)
        {
            var videomsg = new ThumbnailBubble(info);
            videomsg.MaxWidth = 300;
            LeftBubbleFormat?.Invoke(videomsg);
            addMessage(videomsg, toTop);
            msg_scroll.ScrollToEnd();
        }
        #endregion

        private void addMessage(UserControl uc, bool toTop = false) {
            if (toTop)
                message_container.Children.Insert(0, uc);
            else
                message_container.Children.Add(uc);
        }

        #endregion
        
        //Return remove task
        public Action addFakeLoading() {
            var bubble = new ucChatItem(); //TODO
            bubble.text_msg_content.Text = "Loading";

            bubble.HorizontalAlignment = HorizontalAlignment.Right;
            message_container.Children.Add(bubble);

            return () => {
                message_container.Children.Remove(bubble);
            };
        }
        
        public bool trySendTextMessage() {
            string text = ChatInput.Text;
            if (String.IsNullOrEmpty(text)) return false;
            // TextMessage tmp = new TextMessage(text);
            // update_message_container(tmp);
            ChatInput.Text = "";
            module.controller.sendTextMessage(text);
            return true;
        }
        
        public void btnSend_Click(object sender, RoutedEventArgs e) {
            trySendTextMessage();
        }
        
        public void buzz(object sender, RoutedEventArgs e)
        {
            //if (ChatInput.Text != "")
            //{
            //    TextMessage tmp = new TextMessage();
            //    tmp.Message = ChatInput.Text;

            //    update_msgcontainer_at_top(tmp);
            //    update_offline_status();
            //    ChatInput.Text = "";
            //}

            Action<object> buzz = (o) =>
            {
                Action a = () => Left += 10;
                Dispatcher.Invoke(a);
                System.Threading.Thread.Sleep(100);

                a = () => Top -= 6;
                Dispatcher.Invoke(a);
                System.Threading.Thread.Sleep(100);

                a = () => Left -= 4;
                Dispatcher.Invoke(a);
                System.Threading.Thread.Sleep(100);

                a = () => Top += 6;
                Dispatcher.Invoke(a);
                System.Threading.Thread.Sleep(100);

                a = () => Left -= 6;
                Dispatcher.Invoke(a);
                System.Threading.Thread.Sleep(100);
            };
            System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(buzz));
        }
        private double randomDouble(double a, double b)
        {
            Random rand = new Random();
            double result = (rand.NextDouble() * (b - a)) + a;
            return result;
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
                trySendTextMessage();
            }
        }

        public void send_image_on_click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog opendlg = new OpenFileDialog();
            opendlg.Filter = "Image files (*.jpg, *.png) |*.jpg;*.png";
            if (opendlg.ShowDialog() == true)
            {
                ChatContainer container = ModuleContainer.GetModule<ChatContainer>();
                container.controller.sendImageMessage(opendlg.FileNames.ToList());
            }
        }
        private void send_files_on_click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog opendlg = new OpenFileDialog();
            opendlg.Filter = "Video files (*.MP4) |*.MP4";
            if (opendlg.ShowDialog() == true) {
                ChatContainer container = ModuleContainer.GetModule<ChatContainer>();
                container.controller.sendVideoMessage(opendlg.FileNames.ToList());
            }
        }

        #region Status

        public void update_online_status()
        {
            OnlineStatus onl = new OnlineStatus();
            if (Status_container.Children.Count > 0)
            {
                Status_container.Children.RemoveAt(Status_container.Children.Count - 1);
            }
            Status_container.Children.Add(onl);
        }

        public void update_offline_status()
        {
            OfflineStatus off = new OfflineStatus();
            if (Status_container.Children.Count > 0)
            {
                Status_container.Children.RemoveAt(Status_container.Children.Count - 1);
            }
            Status_container.Children.Add(off);
        }
        #endregion

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

        private void Load_chat_page(object sender, RoutedEventArgs e)
        {
            OnlineStatus onlsta = new OnlineStatus();
            Status_container.Children.Add(onlsta);
        }

        private void MediaGalleryBtn_Click(object sender, RoutedEventArgs e)
        {
            //var homewin 
        }

        private void ucInfoConversationSlideBar_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void ChatInfoMenuBtn_Checked(object sender, RoutedEventArgs e)
        {
            HomeWindow homewin = ModuleContainer.GetModule<ChatWindow>().view;
            homewin.OpenProfileDisplayer();
        }

        private void ChatInfoMenuBtn_Unchecked(object sender, RoutedEventArgs e)
        {
            HomeWindow homewin = ModuleContainer.GetModule<ChatWindow>().view;
            homewin.CloseProfileDisplayer();
        }

        private void btnSticker_Click(object sender, RoutedEventArgs e)
        {

        }
        //private void HideNotificationView()
        //{
        //    DoubleAnimation db = new DoubleAnimation();
        //    ChatWindow chatWindow = ModuleContainer.GetModule<ChatWindow>();
        //    ConversationList conver = ModuleContainer.GetModule<ConversationList>();
        //    db.From = conver.view.NotiView.Width;
        //    db.To = 0;
        //    db.Duration = new Duration(TimeSpan.FromSeconds(0.3));
        //    conver.view.NotiView.BeginAnimation(WidthProperty, db);
        //    conver.view.TogglePopupButton.IsChecked = false;
        //    chatWindow.view.ChatPage.Fade.Visibility = Visibility.Hidden;
        //}

        private void closenotibutton_Click(object sender, RoutedEventArgs e)
        {
            //HideNotificationView();
        }
    }
}

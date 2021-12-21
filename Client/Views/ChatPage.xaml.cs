using System;
using System.Collections;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using UI.Components;
using UI.ViewModels;

namespace UI.Views
{
    /// <summary>
    /// Interaction logic for ChatPage.xaml
    /// </summary>
    public partial class ChatPage : UserControl
    {

        public static event Action OnBuzz;

        public static void DoPublicBuzz()
        {
            OnBuzz?.Invoke();
        }

        public ChatPage()
        {
            InitializeComponent();
            OnBuzz += Buzz;
        }

        private double randomDouble(double a, double b) {
            Random rand = new Random();
            double result = (rand.NextDouble() * (b - a)) + a;
            return result;
        }

        public void Buzz()
        {
            var main = App.Current.MainWindow;
            double currLeft = main.Left;
            double currTop = main.Top;
            double buffer = 10;
            Action<object> buzz = (o) => {
                Random rand = new Random();

                //Tuple<double, double> curr =new Tuple<double, double>(main.Top, main.Left);

                Action a = () => {
                    main.Left = currLeft + randomDouble(-buffer, buffer);
                    main.Top = currTop + randomDouble(-buffer, buffer);
                    buffer -= 0.2f;
                };

                for (int i = 0; i <= 50; i++) {
                    Dispatcher.Invoke(a);
                    System.Threading.Thread.Sleep(10);
                }

            };
            System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(buzz));
        }

        
        // Create a custom routed event by first registering a RoutedEventID
        // This event uses the bubbling routing strategy
        public static readonly RoutedEvent InfoCheckedEvent = EventManager.RegisterRoutedEvent(
            nameof(InfoChecked), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ConversationListTabs));
        public static readonly RoutedEvent InfoUncheckedEvent = EventManager.RegisterRoutedEvent(
            nameof(InfoUnchecked), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ConversationListTabs));
        // Provide CLR accessors for the event
        public event RoutedEventHandler InfoChecked
        {
            add { AddHandler(InfoCheckedEvent, value); }
            remove { RemoveHandler(InfoCheckedEvent, value); }
        }
        public event RoutedEventHandler InfoUnchecked
        {
            add { AddHandler(InfoUncheckedEvent, value); }
            remove { RemoveHandler(InfoUncheckedEvent, value); }
        }

        private void OnChecked(object sender, RoutedEventArgs e) {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(InfoCheckedEvent);
            RaiseEvent(newEventArgs);
        }

        private void OnUnchecked(object sender, RoutedEventArgs e) {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(InfoUncheckedEvent);
            RaiseEvent(newEventArgs);
        }

        private void RegisterEvents(object DataContext) {
            if (DataContext == null) return;
            if (DataContext is ConversationViewModel) {
                ConversationViewModel vm = DataContext as ConversationViewModel;
                vm.Messages.CollectionChanged += ItemSourceUpdateEvent;
            }
        }

        private void UnregisterEvents(object DataContext) {
            if (DataContext == null) return;
            if (DataContext is ConversationViewModel) {
                ConversationViewModel vm = DataContext as ConversationViewModel;
                vm.Messages.CollectionChanged -= ItemSourceUpdateEvent;
            }
        }

        private void ItemSourceUpdateEvent(object sender, NotifyCollectionChangedEventArgs args) {
            IList collection = (IList) sender;
            if (collection.Count == 1) {
                ChatScroll.ScrollToBottom();
                return;
            }
            int index = args.NewStartingIndex;
            if (index == 0) {
                //Scroll to top
                if (ChatScroll.VerticalOffset < 1) {
                    
                }
            }
            else {
                //Scroll to bottom
                if (Math.Abs(ChatScroll.VerticalOffset - ChatScroll.ScrollableHeight) < 1) {
                    ChatScroll.ScrollToBottom();
                }
            }
        }

        private void OnDataContextChange(object sender, DependencyPropertyChangedEventArgs e) {
            UnregisterEvents(e.OldValue);
            RegisterEvents(e.NewValue);
        }

        private void OnUnloaded(object sender, RoutedEventArgs e) {
            UnregisterEvents(DataContext);
        }

        private void OfflineStatus_Loaded(object sender, RoutedEventArgs e) {

        }

        private void TextInputChanged(object sender, TextChangedEventArgs e)
        {
            if (ChatInput.Text.Contains(":)"))
            {
                emoReplacement(":)", "🙂");
            }

            if (ChatInput.Text.Contains(":("))
            {
                emoReplacement(":(", "🙁");
            }

            if (ChatInput.Text.Contains("<3"))
            {
                emoReplacement("<3", "❤️");
            }
            if (ChatInput.Text.Contains("-_-"))
            {
                emoReplacement("-_-", "😑");
            }

        }

        private void emoReplacement(string target, string alter)
        {
            Emoji.Wpf.EmojiData.Emoji emoji = new Emoji.Wpf.EmojiData.Emoji();
            if (ChatInput.Text.Contains(target))
            {
                emoji.Text = alter;
                if (emoji != null)
                    ChatInput.Text = ChatInput.Text.Replace(target, emoji.Text);
            }
            ChatInput.Select(ChatInput.Text.Length, 0);
        }

        private void BuzzButton_OnClick(object sender, RoutedEventArgs e)
        {
            Buzz();
        }
    }
}

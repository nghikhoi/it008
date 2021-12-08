using System;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using UI.ViewModels;

namespace UI
{
    /// <summary>
    /// Interaction logic for ChatPage.xaml
    /// </summary>
    public partial class ChatPage : UserControl {

        public ChatPage()
        {
            InitializeComponent();
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
            int index = args.NewStartingIndex;
            if (index == 0) {
                //Scroll to top
                if (ChatScroll.VerticalOffset < 1) {
                    ChatScroll.ScrollToTop();
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
    }
}

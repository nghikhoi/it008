using System.Windows;
using System.Windows.Controls;

namespace UI.Components
{
    /// <summary>
    /// Interaction logic for NotificationPage.xaml
    /// </summary>
    public partial class NotificationPage : UserControl
    {
        public static readonly RoutedEvent ProfileClickEvent = EventManager.RegisterRoutedEvent(
            nameof(ProfileClick), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(NotificationPage));
        public event RoutedEventHandler ProfileClick
        {
            add { AddHandler(ProfileClickEvent, value); }
            remove { RemoveHandler(ProfileClickEvent, value); }
        }
        
        public static readonly RoutedEvent LogoutClickEvent = EventManager.RegisterRoutedEvent(
            nameof(LogoutClick), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(NotificationPage));
        public event RoutedEventHandler LogoutClick
        {
            add { AddHandler(LogoutClickEvent, value); }
            remove { RemoveHandler(LogoutClickEvent, value); }
        }
        
        public NotificationPage() {
            InitializeComponent();
        }

        private void InfOpenBtn_OnClick(object sender, RoutedEventArgs e) {
            RoutedEventArgs args = new RoutedEventArgs(ProfileClickEvent);
            RaiseEvent(args);
        }

        private void LogOutBtn_OnClick(object sender, RoutedEventArgs e) {
            RoutedEventArgs args = new RoutedEventArgs(LogoutClickEvent);
            RaiseEvent(args);
        }
    }
}

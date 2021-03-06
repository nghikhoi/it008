using System;
using System.Windows;
using System.Windows.Controls;

namespace UI.Components
{
    /// <summary>
    /// Interaction logic for FriendRequestNoti.xaml
    /// </summary>
    public partial class FriendRequestNoti : UserControl
    {
        
        public static readonly DependencyProperty SenderNameProperty =
            DependencyProperty.Register(nameof(SenderName), typeof(string), typeof(FriendRequestNoti), new FrameworkPropertyMetadata(null));

        public string SenderName {
            get => GetValue(SenderNameProperty) as string;
            set => SetValue(SenderNameProperty, value);
        }
        
        public FriendRequestNoti()
        {
            InitializeComponent();
        }

        public event EventHandler ClickMask;
        public event EventHandler AcceptClick;
        public event EventHandler DenyClick;
        
        private void ClickMask_Click(object sender, RoutedEventArgs e)
        {
            ClickMask?.Invoke(sender, e);
        }

        private void BtnAccept_OnClick(object sender, RoutedEventArgs e) {
            AcceptClick?.Invoke(sender, e);
        }

        private void RemoveBtn_OnClick(object sender, RoutedEventArgs e) {
            DenyClick?.Invoke(sender, e);
        }
    }
}

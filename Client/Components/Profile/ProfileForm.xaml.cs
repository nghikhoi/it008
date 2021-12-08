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

namespace UI.Components {
    /// <summary>
    /// Interaction logic for SettingForm.xaml
    /// </summary>
    public partial class ProfileForm : UserControl {
        public static readonly RoutedEvent CancelClickEvent = EventManager.RegisterRoutedEvent(
            nameof(CancelClick), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ProfileForm));
        public event RoutedEventHandler CancelClick {
            add { AddHandler(CancelClickEvent, value); }
            remove { RemoveHandler(CancelClickEvent, value); }
        }
        
        public ProfileForm() {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e) {
            RoutedEventArgs args = new RoutedEventArgs(CancelClickEvent);
            RaiseEvent(args);
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using UI.Annotations;

namespace UI
{
    /// <summary>
    /// Interaction logic for NotificationTagItem.xaml
    /// </summary>
    public partial class NotificationTagItem : UserControl, INotifyPropertyChanged
    {
        
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string _Content;

        public string Content {
            get => _Content;
            set {
                _Content = value;
                OnPropertyChanged(nameof(Content));
            }
        }

        public NotificationTagItem()
        {
            InitializeComponent();
        }
        
        public event EventHandler Click;

        private void ClickMask_Click(object sender, RoutedEventArgs e)
        {
            NotificationDot.Visibility = Visibility.Hidden;
        }
        
    }
}

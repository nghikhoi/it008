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
    /// Interaction logic for ChangeNicknameItem.xaml
    /// </summary>
    public partial class ChangeNicknameItem : UserControl {

        public static readonly DependencyProperty NicknameProperty = DependencyProperty.Register(nameof(Nickname), typeof(string)
            , typeof(ChangeNicknameItem), new FrameworkPropertyMetadata(null));

        public string Nickname {
            get => GetValue(NicknameProperty) as string;
            set => SetValue(NicknameProperty, value);
        }

        public ChangeNicknameItem() {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            if (Mask.Visibility == Visibility.Visible)
            {
                Mask.Visibility = Visibility.Hidden;
                Root.Visibility = Visibility.Visible;
                ButtonMask.Visibility = Visibility.Hidden;
            }
        }
    }
}

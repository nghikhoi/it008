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

namespace UI.Components
{
    /// <summary>
    /// Interaction logic for Emoji.xaml
    /// </summary>
    public partial class EmojiMessage : UserControl

    {
        public static readonly DependencyProperty EmoContentProperty = DependencyProperty.Register(nameof(EmoContent), typeof(string)
            , typeof(EmojiMessage), new FrameworkPropertyMetadata(null));

        public string EmoContent
        {
            get => GetValue(EmoContentProperty) as string;
            set => SetValue(EmoContentProperty, value);
        }

        public EmojiMessage()
        {
            InitializeComponent();
        }
    }
}

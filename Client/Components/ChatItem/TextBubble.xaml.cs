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
using System.Windows.Media.Animation;

namespace UI.Components
{
    /// <summary>
    /// Interaction logic for ucChatItem.xaml
    /// </summary>
    public partial class TextBubble : UserControl
    {
        
        public static readonly DependencyProperty ChatContentProperty = DependencyProperty.Register(nameof(ChatContent), typeof(string)
            , typeof(TextBubble), new FrameworkPropertyMetadata(null));

        public string ChatContent {
            get => GetValue(ChatContentProperty) as string;
            set => SetValue(ChatContentProperty, value);
        }
        
        public TextBubble()
        {
            InitializeComponent();
        }
    }
}

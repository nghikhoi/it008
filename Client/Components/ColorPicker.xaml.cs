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
    /// Interaction logic for ColorPicker.xaml
    /// </summary>
    public partial class ColorPicker : UserControl {
        public ColorPicker() {
            InitializeComponent();
        }

        public delegate void ClickHander(Color color);
        public event ClickHander buttonClick;

        public Color Color
        {
            get => (Color) GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }

        public static readonly DependencyProperty ColorProperty =
            DependencyProperty.Register(nameof(Color), typeof(Color), typeof(ColorPicker), new FrameworkPropertyMetadata(Color.FromRgb(0, 0, 0)));

        public static readonly RoutedEvent AcceptEvent = EventManager.RegisterRoutedEvent(
            nameof(Accept), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ColorPicker));

        // Provide CLR accessors for the event
        public event RoutedEventHandler Accept {
            add => AddHandler(AcceptEvent, value); 
            remove => RemoveHandler(AcceptEvent, value); 
        }

        private void AcceptRaise(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(AcceptEvent));
        }
    }
}

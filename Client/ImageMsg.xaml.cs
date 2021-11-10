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

namespace UI
{
    /// <summary>
    /// Interaction logic for ImageMsg.xaml
    /// </summary>
    public partial class ImageMsg : UserControl
    {
        public ImageMsg()
        {
            InitializeComponent();
        }

        private void open_picture_window(object sender, RoutedEventArgs e)
        {
            var imwin = new ImageWindow();
            imwin.ImageControl.Source = ImageControl.Source;
            imwin.Show();
        }

        private void show_control(object sender, MouseEventArgs e)
        {
            DoubleAnimation myDoubleAnimation = new DoubleAnimation();
            myDoubleAnimation.From = 0;
            myDoubleAnimation.To = 0.75;
            myDoubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.25));
            fullsrnicon.BeginAnimation(OpacityProperty, myDoubleAnimation);
        }

        private void hide_control(object sender, MouseEventArgs e)
        {
            DoubleAnimation myDoubleAnimation = new DoubleAnimation();
            myDoubleAnimation.From = 0.75;
            myDoubleAnimation.To = 0.0;
            myDoubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.25));
            fullsrnicon.BeginAnimation(OpacityProperty, myDoubleAnimation);
        }
    }
}

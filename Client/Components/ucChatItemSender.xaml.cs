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
    /// Interaction logic for ucChatItemSender.xaml
    /// </summary>
    public partial class ucChatItemSender : UserControl
    {
        public ucChatItemSender()
        {
            InitializeComponent();
        }

        public void update_Like()
        {
            this.Emotion.Visibility = Visibility.Visible;
            ImageBrush imB = new ImageBrush();
            BitmapImage img = new BitmapImage(new Uri(@"../../IconPack/Like.png", UriKind.Relative));
            imB.ImageSource = img;
            imB.Stretch = Stretch.UniformToFill;
            Emotion.Fill = imB;
        }

        public void update_Heart()
        {
            this.Emotion.Visibility = Visibility.Visible;
            ImageBrush imB = new ImageBrush();
            BitmapImage img = new BitmapImage(new Uri(@"../../IconPack/Heart.png", UriKind.Relative));
            imB.ImageSource = img;
            imB.Stretch = Stretch.UniformToFill;
            Emotion.Fill = imB;
        }

        public void update_Smile()
        {
            this.Emotion.Visibility = Visibility.Visible;
            ImageBrush imB = new ImageBrush();
            BitmapImage img = new BitmapImage(new Uri(@"../../IconPack/Smile.png", UriKind.Relative));
            imB.ImageSource = img;
            imB.Stretch = Stretch.UniformToFill;
            Emotion.Fill = imB;
        }

        public void update_Sad()
        {
            this.Emotion.Visibility = Visibility.Visible;
            ImageBrush imB = new ImageBrush();
            BitmapImage img = new BitmapImage(new Uri(@"../../IconPack/Sad.png", UriKind.Relative));
            imB.ImageSource = img;
            imB.Stretch = Stretch.UniformToFill;
            Emotion.Fill = imB;
        }

        public void update_Angry()
        {
            this.Emotion.Visibility = Visibility.Visible;
            ImageBrush imB = new ImageBrush();
            BitmapImage img = new BitmapImage(new Uri(@"../../IconPack/Angry.png", UriKind.Relative));
            imB.ImageSource = img;
            imB.Stretch = Stretch.UniformToFill;
            Emotion.Fill = imB;
        }
        //private void Like_Checked(object sender, RoutedEventArgs e)
        //{
        //    update_Like();
        //}

        //private void Heart_Checked(object sender, RoutedEventArgs e)
        //{
        //    update_Heart();
        //}

        //private void Smile_Checked(object sender, RoutedEventArgs e)
        //{
        //    update_Smile();
        //}

        //private void Sad_Checked(object sender, RoutedEventArgs e)
        //{
        //    update_Sad();
        //}

        //private void Angry_Checked(object sender, RoutedEventArgs e)
        //{
        //    update_Angry();
        //}

        //private void Grid_MouseEnter(object sender, MouseEventArgs e)
        //{
        //    DoubleAnimation myDoubleAnimation = new DoubleAnimation();
        //    myDoubleAnimation.From = 0.0;
        //    myDoubleAnimation.To = 1.0;
        //    myDoubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.5));
        //    TogglePopupButton.BeginAnimation(OpacityProperty, myDoubleAnimation);
        //}

        //private void Grid_MouseLeave(object sender, MouseEventArgs e)
        //{
        //    DoubleAnimation myDoubleAnimation = new DoubleAnimation();
        //    myDoubleAnimation.From = 1.0;
        //    myDoubleAnimation.To = 0.0;
        //    myDoubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.5));
        //    TogglePopupButton.BeginAnimation(OpacityProperty, myDoubleAnimation);
        //}
    }
}

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

namespace UI
{
    /// <summary>
    /// Interaction logic for VideoMessage.xaml
    /// </summary>
    public partial class VideoMessage : UserControl
    {
        public VideoMessage()
        {
            InitializeComponent();
        }

        private void pause_video_on_click(object sender, RoutedEventArgs e)
        {
            VideoControl.Pause();
        }

        private void play_video_on_click(object sender, RoutedEventArgs e)
        {
            VideoControl.Play();
        }

        private void Loaded_acitivity(object sender, RoutedEventArgs e)
        {
            VideoControl.Position = new TimeSpan(0, 0, 0, 0, 1);
            VideoControl.Pause();
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Stop_video_on_click(object sender, RoutedEventArgs e)
        {
            VideoControl.Stop();
        }
    }
}

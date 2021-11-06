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
    /// Interaction logic for mediaPlayer.xaml
    /// </summary>
    public partial class mediaPlayer : UserControl
    {
        public mediaPlayer()
        {
            InitializeComponent();
        }

        private void open_video_window(object sender, RoutedEventArgs e)
        {
            var mediawin = new VideoWindow();
            Uri video_uri = new Uri(video_link.Text);
            mediawin.VideoControl.Source = video_uri;
            mediawin.Show();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Command
{
    class ImageMsgOpenWin:BaseCommand
    {
        private void open_picture_window(object sender, RoutedEventArgs e)
        {
            var imwin = new ImageWindow();
            imwin.ImageControl.Source = ImageControl.Source;
            imwin.Show();
        }
    }
}

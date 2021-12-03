using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Command
{
    class ImageMsgShowCtrl:BaseCommand
    {
        private void show_control(object sender, MouseEventArgs e)
        {
            DoubleAnimation myDoubleAnimation = new DoubleAnimation();
            myDoubleAnimation.From = 0;
            myDoubleAnimation.To = 0.75;
            myDoubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.25));
            fullsrnicon.BeginAnimation(OpacityProperty, myDoubleAnimation);
        }
    }
}

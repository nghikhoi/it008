using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Command
{
    class ChatItemMouseLeaveCommand:BaseCommand
    {
        private void Grid_MouseLeave(object sender, MouseEventArgs e)
        {
            DoubleAnimation myDoubleAnimation = new DoubleAnimation();
            myDoubleAnimation.From = 1.0;
            myDoubleAnimation.To = 0.0;
            myDoubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.5));
            TogglePopupButton.BeginAnimation(OpacityProperty, myDoubleAnimation);
        }
    }
}

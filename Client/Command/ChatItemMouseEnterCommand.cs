using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Command
{
    public class ChatItemMouseEnterCommand : BaseCommand
    {
        private void Grid_MouseEnter(object sender, MouseEventArgs e)
        {
            DoubleAnimation myDoubleAnimation = new DoubleAnimation();
            myDoubleAnimation.From = 0.0;
            myDoubleAnimation.To = 1.0;
            myDoubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.5));
            TogglePopupButton.BeginAnimation(OpacityProperty, myDoubleAnimation);
        }
    }
}

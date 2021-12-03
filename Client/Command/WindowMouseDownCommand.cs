using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UI
{
    public class WindowMouseDownCommand : Command.BaseCommand
    {
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
    }
}
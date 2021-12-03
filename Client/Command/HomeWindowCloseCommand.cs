using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Command
{
    class HomeWindowCloseCommand:BaseCommand
    {
        private void Window_Closed(object sender, EventArgs e)
        {
            foreach (Window win in Application.Current.Windows)
            {
                if (win is IView)
                    win.Hide();
                else win.Close();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Command
{
    class ChatItemSmileCommand:BaseCommand
    {
        private void Smile_Checked(object sender, RoutedEventArgs e)
        {
            update_Smile();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Command
{
    class ChatItemHeartCommand:BaseCommand
    {
        private void Heart_Checked(object sender, RoutedEventArgs e)
        {
            update_Heart();
        }
    }
}

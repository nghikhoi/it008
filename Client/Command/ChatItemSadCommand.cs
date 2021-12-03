using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Command
{
    class ChatItemSadCommand:BaseCommand
    {
        private void Sad_Checked(object sender, RoutedEventArgs e)
        {
            update_Sad();
        }
    }
}

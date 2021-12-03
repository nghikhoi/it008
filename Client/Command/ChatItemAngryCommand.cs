using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Command
{
    class ChatItemAngryCommand:BaseCommand
    {
        private void Angry_Checked(object sender, RoutedEventArgs e)
        {
            update_Angry();
        }
    }
}

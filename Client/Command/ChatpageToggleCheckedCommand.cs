using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Command
{
    class ChatpageToggleCheckedCommand:BaseCommand
    {
        private void ChatInfoMenuBtn_Checked(object sender, RoutedEventArgs e)
        {
            HomeWindow homewin = ModuleContainer.GetModule<ChatWindow>().view;
            homewin.OpenProfileDisplayer();
        }
    }
}

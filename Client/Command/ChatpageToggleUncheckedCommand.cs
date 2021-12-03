using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Command
{
    class ChatpageToggleUncheckedCommand:BaseCommand
    {
        private void ChatInfoMenuBtn_Unchecked(object sender, RoutedEventArgs e)
        {
            HomeWindow homewin = ModuleContainer.GetModule<ChatWindow>().view;
            homewin.CloseProfileDisplayer();
        }
    }
}

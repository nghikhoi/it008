using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Command
{
    class NotificationLogoutCommand:BaseCommand
    {
        private void LogOutBtn_Click(object sender, RoutedEventArgs e)
        {
            ChatWindow chatWindow = ModuleContainer.GetModule<ChatWindow>();
            chatWindow.controller.LogOut();
        }
    }
}

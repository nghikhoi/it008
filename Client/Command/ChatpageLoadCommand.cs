using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Command
{
    class ChatpageLoadCommand:BaseCommand
    {
        private void Load_chat_page(object sender, RoutedEventArgs e)
        {
            OnlineStatus onlsta = new OnlineStatus();
            Status_container.Children.Add(onlsta);
        }
    }
}

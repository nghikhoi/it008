using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Command
{
    class ChatpageSendClickCommand:BaseCommand
    {
        public void btnSend_Click(object sender, RoutedEventArgs e)
        {
            trySendTextMessage();
        }
    }
}

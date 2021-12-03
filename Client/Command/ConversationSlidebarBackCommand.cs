using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Command
{
    class ConversationSlidebarBackCommand:BaseCommand
    {
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            transitioner.SelectedIndex = 0;
        }
    }
}

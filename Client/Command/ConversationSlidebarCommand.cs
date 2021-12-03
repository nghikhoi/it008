using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Command
{
    class ConversationSlidebarCommand:BaseCommand
    {
        private void BtnMediaGallery_Click(object sender, RoutedEventArgs e)
        {
            transitioner.SelectedIndex = 1;
        }
    }
}

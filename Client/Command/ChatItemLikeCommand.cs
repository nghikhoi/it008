using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Command
{
    public class ChatItemLikeCommand : BaseCommand
    {
        private void Like_Checked(object sender, RoutedEventArgs e)
        {
            update_Like();
        }
    }
}

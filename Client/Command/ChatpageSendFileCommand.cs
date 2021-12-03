using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Command
{
    class ChatpageSendFileCommand:BaseCommand
    {
        private void send_files_on_click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog opendlg = new OpenFileDialog();
            opendlg.Filter = "Video files (*.MP4) |*.MP4";
            if (opendlg.ShowDialog() == true)
            {
                ChatContainer container = ModuleContainer.GetModule<ChatContainer>();
                container.controller.sendVideoMessage(opendlg.FileNames.ToList());
            }
        }
    }
}

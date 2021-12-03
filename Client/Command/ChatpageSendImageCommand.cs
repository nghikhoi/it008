using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Command
{
    class ChatpageSendImageCommand:BaseCommand
    {
        public void send_image_on_click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog opendlg = new OpenFileDialog();
            opendlg.Filter = "Image files (*.jpg, *.png) |*.jpg;*.png";
            if (opendlg.ShowDialog() == true)
            {
                ChatContainer container = ModuleContainer.GetModule<ChatContainer>();
                container.controller.sendImageMessage(opendlg.FileNames.ToList());
            }
        }
    }
}

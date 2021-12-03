using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//todo: nut back cho sign up 
namespace UI.Command
{
    public class SignUpBackClickCommand : Command.BaseCommand
    {
        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            AuthenticationController controller = ModuleContainer.GetModule<Authentication>().controller;
            //OnContentRendered(e);
            controller.showLogin();
        }
    }
}
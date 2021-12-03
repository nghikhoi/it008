using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Command
{
    class NotificationInfoCommand:BaseCommand
    {
        private void InfOpenBtn_Click(object sender, RoutedEventArgs e)
        {
            var parentwin = Window.GetWindow(this) as HomeWindow;
            parentwin.make_fade();
            SettingPage settingPage = ModuleContainer.GetModule<SettingPage>();
            settingPage.view.Show();
        }
    }
}

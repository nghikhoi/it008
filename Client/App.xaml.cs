using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using UI.Utils;

namespace UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            AppConfig.StartService();
            WindowLogIn logIn = new WindowLogIn();
            //MediaPlayerWindow main = new MediaPlayerWindow();
            //MainWindow main = new MainWindow();
            //TestWindows main = new TestWindows();
            //DownloadWindow main = new DownloadWindow();
            logIn.Show();
           
        }

    }
}

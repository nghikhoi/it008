using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using UI.Utils;

namespace UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {

        private readonly String defaultLanguage = "vi-VN";
        private readonly List<String> availableLanguage = new List<string>() {
            "en-US",
            "vi-VN"
        };
        
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            AppConfig.StartService();
            ApplyLanguage("en-US");
            WindowLogIn logIn = new WindowLogIn();
            //MediaPlayerWindow main = new MediaPlayerWindow();
            //MainWindow main = new MainWindow();
            //TestWindows main = new TestWindows();
            //DownloadWindow main = new DownloadWindow();
            logIn.Show();
        }
        
        public void ApplyLanguage(string cultureName = null) {
            if (cultureName != null)
                Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(cultureName);

            ResourceDictionary dict = new ResourceDictionary();
            String langKey = Thread.CurrentThread.CurrentCulture.ToString();
            if (!availableLanguage.Contains(langKey))
                langKey = defaultLanguage;
            dict.Source = new Uri("..\\Lang\\" + langKey + ".xaml", UriKind.Relative);

            Resources.MergedDictionaries.Add(dict);
        }

    }
}

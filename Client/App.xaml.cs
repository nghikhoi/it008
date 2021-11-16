using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using UI.MVC;
using UI.Network.Packets.AfterLoginRequest.Notification;
using UI.Utils;

namespace UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {

        public static readonly bool IS_LOCAL_DEBUG = false;

        private readonly String defaultLanguage = "vi-VN";
        private readonly List<String> availableLanguage = new List<string>() {
            "en-US",
            "vi-VN"
        };
        public static App instance;
        public App()
        {
            instance = this;
        }
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            AppConfig.StartService();
            ApplyLanguage(defaultLanguage);
            initAuthentication();
            //MediaPlayerWindow main = new MediaPlayerWindow();
            //MainWindow main = new MainWindow();
            //TestWindows main = new TestWindows();
            //DownloadWindow main = new DownloadWindow();
            ModuleContainer.GetModule<Authentication>().controller.showLogin();
        }

        public void initAuthentication() {
            WindowLogIn logIn = new WindowLogIn();
            WindowRegister register = new WindowRegister();

            AuthenticationController controller = new AuthenticationController(logIn, register);
            AuthenticationView view = new AuthenticationView(logIn, register);
            Authentication module = new Authentication();
            module.InitializeMVC(ChatModel.Instance, view, controller);
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

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using UI.Lang;
using UI.MVC;
using UI.Network;
using UI.Network.Packets.AfterLoginRequest.Notification;
using UI.Utils;
using MaterialDesignThemes.Wpf;
using UI.Properties;

namespace UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {

        public static readonly bool IS_LOCAL_DEBUG = true;
        public static readonly bool SESSION_HOLDER = false;
        
        public static App Instance { get; private set; }
        public App()
        {
            Instance = this;
        }
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            AppConfig.StartService();
            Language.ApplyLanguage(Language.defaultLanguage);
            //Instance.SetTheme(Settings.Default.Theme);
            initAuthentication();
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

        public void openHomeWindow(string hash) {
            ChatModel.Instance.Hashed = hash;
            initHomeWindow();
            ChatWindow module = ModuleContainer.GetModule<ChatWindow>();
            module.controller.showView();
        }
        
        public void initHomeWindow() {
            HomeWindow view = new HomeWindow();
            ChatWindowController controller = new ChatWindowController(view);
            ChatWindow module = new ChatWindow();
            module.InitializeMVC(ChatModel.Instance, view, controller);
        }

        //public void exit() {
        //    Environment.Exit(0);
        //}
        
        //private bool _isMaximized;
        //public bool isMaximized
        //{
        //    get
        //    {
        //        return _isMaximized;
        //    }
        //    set
        //    {
        //        _isMaximized = value;
        //        Application.Current.MainWindow.WindowState = _isMaximized == false ? WindowState.Normal : WindowState.Maximized;
        //    }
        //}

        //public void changeMaximized() {
        //    isMaximized = !isMaximized;
        //}

        //public void minimized() {
        //    Application.Current.MainWindow.WindowState = WindowState.Minimized;
        //}

        public void SetTheme(BaseTheme theme)
        {

            //string lightSource = "pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Light.xaml";
            //string darkSource = "pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Dark.xaml";

            //foreach (ResourceDictionary resourceDictionary in Resources.MergedDictionaries)
            //{
            //    if (string.Equals(resourceDictionary.Source?.ToString(), lightSource, StringComparison.OrdinalIgnoreCase)
            //        || string.Equals(resourceDictionary.Source?.ToString(), darkSource, StringComparison.OrdinalIgnoreCase))
            //    {
            //        Resources.MergedDictionaries.Remove(resourceDictionary);
            //        break;
            //    }
            //}
            try
            {
                if (theme == BaseTheme.Dark)
                {
                    //Resources.MergedDictionaries.Insert(0, new ResourceDictionary { Source = new Uri(darkSource) });
                    ChangeTheme(new Uri($"pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.{"Dark"}.xaml", UriKind.RelativeOrAbsolute));
                }
                else
                {
                    //This handles both Light and Inherit
                    //Resources.MergedDictionaries.Insert(0, new ResourceDictionary { Source = new Uri(lightSource) });
                    ChangeTheme(new Uri($"pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.{"Light"}.xaml", UriKind.RelativeOrAbsolute));
                }
            }
            catch {
                MessageBox.Show("dum", "chan");
            }
        }

        void ChangeTheme(Uri uri)
        {
            Resources.MergedDictionaries[0] = new ResourceDictionary { Source = uri };
        }
    }
}

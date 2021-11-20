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
using UI.Network.Packets.AfterLoginRequest.Notification;
using UI.Utils;

namespace UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {

        public static readonly bool IS_LOCAL_DEBUG = true;
        
        public static App Instance { get; private set; }
        public App()
        {
            Instance = this;
        }
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            AppConfig.StartService();
            Language.ApplyLanguage(Language.defaultLanguage);
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

    }
}

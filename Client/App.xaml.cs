using System;
using System.Windows;
using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UI.Services;
using UI.Services.Common;
using UI.Views;

namespace UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {

        public static readonly bool IS_LOCAL_DEBUG = false;
        public static readonly bool SESSION_HOLDER = false;
        
        public static App Instance { get; private set; }
        private readonly IHost _host;
        
        public App()
        {
            Instance = this;
            _host = CreateHostBuilder().Build();
        }
        
        public static IHostBuilder CreateHostBuilder(string[] args = null) {
            return Host.CreateDefaultBuilder(args)
                .AddNetwork()
                .AddStores()
                .AddModels()
                .AddViewModels()
                .AddViews();
        }
        
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // AppConfig.StartService();
            Language.ApplyLanguage(Language.defaultLanguage);
            _host.Services.GetRequiredService<MainWindowNavigator<AuthenticationWindow>>().Navigate();
        }
        
    }
}

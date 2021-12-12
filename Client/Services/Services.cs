using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using UI.Command;
using UI.Network;
using UI.Network.Protocol;
using UI.Services.Common;
using UI.ViewModels;

namespace UI.Services {
    public static class Services {

        public static IHostBuilder AddStores(this IHostBuilder host) {
            host.ConfigureServices(services => {
                services.AddSingleton<IViewState, ViewState>();
                services.AddSingleton<IAppSession, AppSession>();
            });
            return host;
        }

        public static IHostBuilder AddNetwork(this IHostBuilder host) {
            host.ConfigureServices(services => {
                services.AddSingleton<RespondeManager>();
                services.AddSingleton<PacketRespondeListener>();
                services.AddSingleton<ProtocolProvider>();
                services.AddSingleton<ChatConnection>();
                services.AddSingleton<IAuthenticator, Authenticator>();
                services.AddSingleton<IUserProfileHolder, UserProfileHolder>();
            });
            return host;
        }

        public static IHostBuilder AddViewModels(this IHostBuilder host) {
            host.ConfigureServices(services => {
                services.AddTransient<ConversationViewModel>();
                services.AddTransient<FriendConversationViewModel>();
                services.AddTransient<TextMessageViewModel>();
                services.AddTransient<VideoMessageViewModel>();
                services.AddTransient<ImageMessageViewModel>();
                services.AddTransient<HomeViewModel>();
                services.AddTransient<ProfileViewModel>();
                
                services.AddSingleton<AuthenticationViewModel>();
                services.AddSingleton<ViewModelCreator<ConversationViewModel>>(s => s.GetRequiredService<ConversationViewModel>);
                services.AddSingleton<ViewModelCreator<FriendConversationViewModel>>(s => s.GetRequiredService<FriendConversationViewModel>);
                services.AddSingleton<ViewModelCreator<TextMessageViewModel>>(s => s.GetRequiredService<TextMessageViewModel>);
                services.AddSingleton<ViewModelCreator<VideoMessageViewModel>>(s => s.GetRequiredService<VideoMessageViewModel>);
                services.AddSingleton<ViewModelCreator<ImageMessageViewModel>>(s => s.GetRequiredService<ImageMessageViewModel>);
                services.AddSingleton<ViewModelCreator<HomeViewModel>>(s => s.GetRequiredService<HomeViewModel>);
                services.AddSingleton<ViewModelCreator<ProfileViewModel>>(s => s.GetRequiredService<ProfileViewModel>);
                services.AddSingleton<ViewModelCreator<LoginViewModel>>(s => () => CreateLoginViewModel(s));
                services.AddSingleton<ViewModelCreator<RegisterViewModel>>(s => () => CreateRegisterViewModel(s));

                services.AddSingleton<IViewModelFactory, ViewModelFactory>();
                services.AddSingleton<ViewStateDelegateNavigator<HomeViewModel>>();
                services.AddTransient<MainViewModel>();
            });
            return host;
        }
        
        public static IHostBuilder AddViews(this IHostBuilder host) {
            host.ConfigureServices(services => {
                services.AddSingleton<Func<MainWindow>>(s => () => new MainWindow(s.GetRequiredService<MainViewModel>()));
                services.AddSingleton<MainWindowNavigator<MainWindow>>();
                services.AddSingleton<Func<AuthenticationWindow>>(s => () => {
                    AuthenticationViewModel authenticationViewModel = s.GetRequiredService<AuthenticationViewModel>();
                    IViewModelFactory factory = s.GetRequiredService<IViewModelFactory>();
                    UpdateCurrentViewModelCommand<LoginViewModel> loginCommand = new UpdateCurrentViewModelCommand<LoginViewModel>(authenticationViewModel, factory);
                    loginCommand.Execute();
                    return new AuthenticationWindow(authenticationViewModel);
                });
                services.AddSingleton<MainWindowNavigator<AuthenticationWindow>>();
            });
            return host;
        }
        
        private static LoginViewModel CreateLoginViewModel(IServiceProvider services)
        { 
            return new LoginViewModel(
                services.GetRequiredService<IAuthenticator>(),
                services.GetRequiredService<AuthenticationViewModel>(),
                services.GetRequiredService<MainWindowNavigator<MainWindow>>(),
                services.GetRequiredService<IViewModelFactory>());
        }

        private static RegisterViewModel CreateRegisterViewModel(IServiceProvider services)
        {
            return new RegisterViewModel(
                services.GetRequiredService<AuthenticationViewModel>(),
                services.GetRequiredService<IViewModelFactory>(),
                services.GetRequiredService<IAuthenticator>());
        }

    }
}

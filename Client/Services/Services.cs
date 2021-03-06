using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using log4net.Config;
using Microsoft.Extensions.Logging;
using UI.Command;
using UI.Models;
using UI.Models.Impl;
using UI.Models.Message;
using UI.Network;
using UI.Network.Protocol;
using UI.Services.Common;
using UI.ViewModels;
using UI.ViewModels.Search;
using UI.Views;

namespace UI.Services {
    public static class Services {

        public static IHostBuilder AddLogging(this IHostBuilder host)
        {
            host.ConfigureLogging(builder => {
                builder.AddLog4Net("log4net.config");
            });
            return host;
        }

        public static IHostBuilder AddStores(this IHostBuilder host) {
            host.ConfigureServices(services => {
                services.AddSingleton<IViewState, ViewState>();
                services.AddSingleton<IAppSession, AppSession>();
            });
            return host;
        }

        public static IHostBuilder AddNetwork(this IHostBuilder host) {
            host.ConfigureServices(services =>
            {
                services.AddSingleton<RespondeManager>();
                services.AddSingleton<PacketRespondeListener>();
                services.AddSingleton<ProtocolProvider>();
                services.AddSingleton<ChatConnection>();
                services.AddSingleton<IAuthenticator, Authenticator>();
                services.AddSingleton<IUserProfileHolder, UserProfileHolder>();
            });
            return host;
        }

        public static IHostBuilder AddModels(this IHostBuilder host) {
            host.ConfigureServices(services =>
            {
                services.AddTransient<AbstractConversation>();
                services.AddTransient<GroupConversation>();

                services.AddSingleton<ModelCreator<AbstractConversation>>(s => s.GetRequiredService<AbstractConversation>);
                services.AddSingleton<ModelCreator<GroupConversation>>(s => s.GetRequiredService<GroupConversation>);

                services.AddSingleton<IModelFactory, ModelFactory>();
                services.AddSingleton<IModelContext, ModelContext>();
            });
            return host;
        }

        public static IHostBuilder AddViewModels(this IHostBuilder host) {
            host.ConfigureServices(services => {
                services.AddTransient<ConversationViewModel>();
                services.AddTransient<FriendConversationViewModel>();
                services.AddTransient<GroupConversationViewModel>();
                services.AddTransient<TextMessageViewModel>();
                services.AddTransient<VideoMessageViewModel>();
                services.AddTransient<ImageMessageViewModel>();
                services.AddTransient<StickerMessageViewModel>();
                services.AddTransient<AttachmentMessageViewModel>();
                services.AddTransient<AnnouncementViewModel>();
                services.AddTransient<HomeViewModel>();
                services.AddTransient<ProfileViewModel>();
                services.AddTransient<NotificationPageViewModel>();
                services.AddTransient<FriendRequestNotificationViewModel>();
                services.AddTransient<FriendAccpectedNotificationViewModel>();
                services.AddTransient<StickerContainerViewModel>();
                services.AddTransient<StickerItemStoreViewModel>();
                services.AddTransient<StickerRecentTabViewModel>();
                services.AddTransient<StickerStoreViewModel>();
                services.AddTransient<StickerOwnedTabViewModel>();
                services.AddTransient<SearchViewModel>();

                services.AddSingleton<AuthenticationViewModel>();
                services.AddSingleton<HomeDialogState>();
                services.AddSingleton<ViewModelCreator<ConversationViewModel>>(s => s.GetRequiredService<ConversationViewModel>);
                services.AddSingleton<ViewModelCreator<FriendConversationViewModel>>(s => s.GetRequiredService<FriendConversationViewModel>);
                services.AddSingleton<ViewModelCreator<GroupConversationViewModel>>(s => s.GetRequiredService<GroupConversationViewModel>);
                services.AddSingleton<ViewModelCreator<TextMessageViewModel>>(s => s.GetRequiredService<TextMessageViewModel>);
                services.AddSingleton<ViewModelCreator<VideoMessageViewModel>>(s => s.GetRequiredService<VideoMessageViewModel>);
                services.AddSingleton<ViewModelCreator<ImageMessageViewModel>>(s => s.GetRequiredService<ImageMessageViewModel>);
                services.AddSingleton<ViewModelCreator<StickerMessageViewModel>>(s => s.GetRequiredService<StickerMessageViewModel>);
                services.AddSingleton<ViewModelCreator<AttachmentMessageViewModel>>(s => s.GetRequiredService<AttachmentMessageViewModel>);
                services.AddSingleton<ViewModelCreator<AnnouncementViewModel>>(s => s.GetRequiredService<AnnouncementViewModel>);
                services.AddSingleton<ViewModelCreator<HomeViewModel>>(s => s.GetRequiredService<HomeViewModel>);
                services.AddSingleton<ViewModelCreator<ProfileViewModel>>(s => s.GetRequiredService<ProfileViewModel>);
                services.AddSingleton<ViewModelCreator<NotificationPageViewModel>>(s => s.GetRequiredService<NotificationPageViewModel>);
                services.AddSingleton<ViewModelCreator<FriendRequestNotificationViewModel>>(s => s.GetRequiredService<FriendRequestNotificationViewModel>);
                services.AddSingleton<ViewModelCreator<FriendAccpectedNotificationViewModel>>(s => s.GetRequiredService<FriendAccpectedNotificationViewModel>);
                services.AddSingleton<ViewModelCreator<StickerContainerViewModel>>(s => s.GetRequiredService<StickerContainerViewModel>);
                services.AddSingleton<ViewModelCreator<StickerItemStoreViewModel>>(s => s.GetRequiredService<StickerItemStoreViewModel>);
                services.AddSingleton<ViewModelCreator<StickerRecentTabViewModel>>(s => s.GetRequiredService<StickerRecentTabViewModel>);
                services.AddSingleton<ViewModelCreator<StickerStoreViewModel>>(s => s.GetRequiredService<StickerStoreViewModel>);
                services.AddSingleton<ViewModelCreator<StickerOwnedTabViewModel>>(s => s.GetRequiredService<StickerOwnedTabViewModel>);
                services.AddSingleton<ViewModelCreator<SearchViewModel>>(s => s.GetRequiredService<SearchViewModel>);
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

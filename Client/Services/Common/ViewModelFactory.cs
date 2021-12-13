using System;
using UI.ViewModels;

namespace UI.Services.Common {
	public class ViewModelFactory : IViewModelFactory {
		private readonly AuthenticationViewModel _authenticationViewModel;
		private readonly ViewModelCreator<RegisterViewModel> _registerCreator;
		private readonly ViewModelCreator<LoginViewModel> _loginCreator;
		private readonly ViewModelCreator<ConversationViewModel> _conversationCreator;
		private readonly ViewModelCreator<FriendConversationViewModel> _friendConversationCreator;
		private readonly ViewModelCreator<TextMessageViewModel> _textMessageCreator;
		private readonly ViewModelCreator<VideoMessageViewModel> _videoMessageCreator;
		private readonly ViewModelCreator<ImageMessageViewModel> _imageMessageCreator;
		private readonly ViewModelCreator<HomeViewModel> _homeCreator;
		private readonly ViewModelCreator<ProfileViewModel> _profileCreator;
        private readonly ViewModelCreator<NotificationPageViewModel> _notificationPageCreator;
        private readonly ViewModelCreator<FriendRequestNotificationViewModel> _friendRequestCreator;
        private readonly ViewModelCreator<FriendAccpectedNotificationViewModel> _friendAcceptedCreator;
        private readonly ViewModelCreator<StickerContainerViewModel> _stickerContainerCreator;
		private readonly ViewModelCreator<StickerItemStoreViewModel> _stickerItemStoreCreator;
		private readonly ViewModelCreator<StickerStoreViewModel> _stickerStoreCreator;
		private readonly ViewModelCreator<StickerOwnedTabViewModel> _stickerOwnedTabCreator;
		private readonly ViewModelCreator<StickerRecentTabViewModel> _stickerRecentTabCreator;

        public ViewModelFactory(AuthenticationViewModel authenticationViewModel, ViewModelCreator<RegisterViewModel> registerCreator, ViewModelCreator<LoginViewModel> loginCreator, ViewModelCreator<ConversationViewModel> conversationCreator, ViewModelCreator<FriendConversationViewModel> friendConversationCreator, ViewModelCreator<TextMessageViewModel> textMessageCreator, ViewModelCreator<VideoMessageViewModel> videoMessageCreator, ViewModelCreator<ImageMessageViewModel> imageMessageCreator, ViewModelCreator<HomeViewModel> homeCreator, ViewModelCreator<ProfileViewModel> profileCreator, ViewModelCreator<NotificationPageViewModel> notificationPageCreator, ViewModelCreator<FriendRequestNotificationViewModel> friendRequestCreator, ViewModelCreator<FriendAccpectedNotificationViewModel> friendAcceptedCreator, ViewModelCreator<StickerContainerViewModel> stickerContainerCreator, ViewModelCreator<StickerItemStoreViewModel> stickerItemStoreCreator, ViewModelCreator<StickerStoreViewModel> stickerStoreCreator, ViewModelCreator<StickerOwnedTabViewModel> stickerOwnedTabCreator, ViewModelCreator<StickerRecentTabViewModel> stickerRecentTabCreator)
        {
            _authenticationViewModel = authenticationViewModel;
            _registerCreator = registerCreator;
            _loginCreator = loginCreator;
            _conversationCreator = conversationCreator;
            _friendConversationCreator = friendConversationCreator;
            _textMessageCreator = textMessageCreator;
            _videoMessageCreator = videoMessageCreator;
            _imageMessageCreator = imageMessageCreator;
            _homeCreator = homeCreator;
            _profileCreator = profileCreator;
            _notificationPageCreator = notificationPageCreator;
            _friendRequestCreator = friendRequestCreator;
            _friendAcceptedCreator = friendAcceptedCreator;
            _stickerContainerCreator = stickerContainerCreator;
            _stickerItemStoreCreator = stickerItemStoreCreator;
            _stickerStoreCreator = stickerStoreCreator;
            _stickerOwnedTabCreator = stickerOwnedTabCreator;
            _stickerRecentTabCreator = stickerRecentTabCreator;
        }

        public TViewModel Create<TViewModel>() where TViewModel : ViewModelBase {
			Type type = typeof(TViewModel);
			if (type == typeof(AuthenticationViewModel))
				return (TViewModel) Convert.ChangeType(_authenticationViewModel, type);
			if (type == typeof(RegisterViewModel))
				return (TViewModel) Convert.ChangeType(_registerCreator.Invoke(), type);
			if (type == typeof(LoginViewModel))
				return (TViewModel) Convert.ChangeType(_loginCreator.Invoke(), type);
			if (type == typeof(HomeViewModel))
				return (TViewModel) Convert.ChangeType(_homeCreator.Invoke(), type);
			if (type == typeof(ConversationViewModel))
				return (TViewModel) Convert.ChangeType(_conversationCreator.Invoke(), type);
			if (type == typeof(FriendConversationViewModel))
				return (TViewModel) Convert.ChangeType(_friendConversationCreator.Invoke(), type);
			if (type == typeof(TextMessageViewModel))
				return (TViewModel) Convert.ChangeType(_textMessageCreator.Invoke(), type);
			if (type == typeof(VideoMessageViewModel))
				return (TViewModel) Convert.ChangeType(_videoMessageCreator.Invoke(), type);
			if (type == typeof(ImageMessageViewModel))
				return (TViewModel) Convert.ChangeType(_imageMessageCreator.Invoke(), type);
			if (type == typeof(ProfileViewModel))
				return (TViewModel) Convert.ChangeType(_profileCreator.Invoke(), type);
			if (type == typeof(NotificationPageViewModel))
                return (TViewModel) Convert.ChangeType(_notificationPageCreator.Invoke(), type);
			if (type == typeof(FriendRequestNotificationViewModel))
				return (TViewModel) Convert.ChangeType(_friendRequestCreator.Invoke(), type);
			if (type == typeof(FriendAccpectedNotificationViewModel))
				return (TViewModel) Convert.ChangeType(_friendAcceptedCreator.Invoke(), type);
            if (type == typeof(StickerContainerViewModel))
                return (TViewModel) Convert.ChangeType(_stickerContainerCreator.Invoke(), type);
            if (type == typeof(StickerItemStoreViewModel))
                return (TViewModel) Convert.ChangeType(_stickerItemStoreCreator.Invoke(), type);
            if (type == typeof(StickerStoreViewModel))
                return (TViewModel) Convert.ChangeType(_stickerStoreCreator.Invoke(), type);
            if (type == typeof(StickerOwnedTabViewModel))
                return (TViewModel) Convert.ChangeType(_stickerOwnedTabCreator.Invoke(), type);
            if (type == typeof(StickerRecentTabViewModel))
                return (TViewModel) Convert.ChangeType(_stickerRecentTabCreator.Invoke(), type);
			return null;
		}
	}
}
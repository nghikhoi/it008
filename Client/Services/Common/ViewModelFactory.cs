using System;
using UI.ViewModels;
using UI.ViewModels.Authentication;
using UI.ViewModels.Messages;

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

		public ViewModelFactory(AuthenticationViewModel authenticationViewModel, ViewModelCreator<RegisterViewModel> registerCreator, ViewModelCreator<LoginViewModel> loginCreator, ViewModelCreator<ConversationViewModel> conversationCreator, ViewModelCreator<FriendConversationViewModel> friendConversationCreator, ViewModelCreator<TextMessageViewModel> textMessageCreator, ViewModelCreator<VideoMessageViewModel> videoMessageCreator, ViewModelCreator<ImageMessageViewModel> imageMessageCreator, ViewModelCreator<HomeViewModel> homeCreator, ViewModelCreator<ProfileViewModel> profileCreator) {
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
			return null;
		}
	}
}
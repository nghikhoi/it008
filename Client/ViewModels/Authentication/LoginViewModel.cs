using System.Windows.Input;
using MaterialDesignThemes.Wpf;
using UI.Command;
using UI.Components;
using UI.Models;
using UI.Services;
using UI.Services.Exceptions;
using UI.Utils;

namespace UI.ViewModels {
	public class LoginViewModel : ViewModelBase {

		private string _username;
		public string Username {
			get => _username;
			set {
				_username = value;
				OnPropertyChanged("Username");
			}
		}
        public string _password;
		public string Password {
            get => _password;
			set {
				_password = value;
				OnPropertyChanged("Password");
			}
		}

		public ICommand LoginCommand { get; set; }
		public ICommand OpenRegisterCommand { get; set; }

		private INavigator _loginSuccessNavigator;
		private readonly IAuthenticator _authenticator;

		public LoginViewModel(IAuthenticator authenticator, AuthenticationViewModel authenticationViewModel, INavigator loginSuccessNavigator, IViewModelFactory factory) {
			_authenticator = authenticator;
			_loginSuccessNavigator = loginSuccessNavigator;
			LoginCommand = new RelayCommand<object>(o => CanLogin(), o => Login());
			OpenRegisterCommand = new UpdateCurrentViewModelCommand<RegisterViewModel>(authenticationViewModel, factory);
		}
		
		public bool CanLogin() {
			return true;//!string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password); //TODO
		}

		public async void Login() {
			try {
                if (!FastCodeUtils.NotEmptyStrings(Username, Password)) {
                    await DialogHost.Show(new AnnouncementDialog(Language.getText("Announce.NeedAcc")));
                    return;
                }
				if (!App.IS_LOCAL_DEBUG)
					await _authenticator.Login(new LoginInfo(Username, Password));
				_loginSuccessNavigator.Navigate();
			}
			catch (UserNotFoundException e) {
                await DialogHost.Show(new AnnouncementDialog(Language.getText("Announce.WrongAcc")));
			}
			catch (InvalidPasswordException e) {
                await DialogHost.Show(new AnnouncementDialog(Language.getText("Announce.WrongPass")));
			}
		}

	}
}
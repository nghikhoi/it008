using UI.Models;
using System;
using System.Windows.Input;
using UI.Command;
using UI.Services;
using UI.Services.Common;
using UI.Services.Exceptions;

namespace UI.ViewModels.Authentication {
	public class RegisterViewModel : ViewModelBase {

		private string _username;
		public string Username {
			get => _username;
			set {
				_username = value;
				OnPropertyChanged(nameof(Username));
			}
		}
		private string _password;
		public string Password {
			get => _password;
			set {
				_password = value;
				OnPropertyChanged(nameof(Password));
			}
		}
		private string _repeatPassword;
		public string RepeatPassword {
			get => _repeatPassword;
			set {
				_repeatPassword = value;
				OnPropertyChanged(nameof(RepeatPassword));
			}
		}
		private string _firstName;
		public string FirstName {
			get => _firstName;
			set {
				_firstName = value;
				OnPropertyChanged(nameof(FirstName));
			}
		}
		private string _lastName;
		public string LastName {
			get => _lastName;
			set {
				_lastName = value;
				OnPropertyChanged(nameof(LastName));
			}
		}
		private Gender _gender;
		public Gender Gender {
			get => _gender;
			set {
				_gender = value;
				OnPropertyChanged(nameof(Gender));
			}
		}
		private DateTime _dayOfBirth;
		public DateTime DayOfBirth {
			get => _dayOfBirth;
			set {
				_dayOfBirth = value;
				OnPropertyChanged(nameof(DayOfBirth));
			}
		}

		public ICommand RegisterCommand { get; set; }
		public ICommand OpenLoginCommand { get; set; }
		
		private readonly IAuthenticator _authenticator;

		public RegisterViewModel(AuthenticationViewModel authenticationViewModel, IViewModelFactory factory, IAuthenticator authenticator) {
			OpenLoginCommand = new UpdateCurrentViewModelCommand<LoginViewModel>(authenticationViewModel, factory);
			_authenticator = authenticator;
			RegisterCommand = new RelayCommand<object>(o => CanRegister(), o => Register());
		}
		
		public bool CanRegister() {
			return true; 
			/*!string.IsNullOrEmpty(Username)
			       && !string.IsNullOrEmpty(Password)
			       && !string.IsNullOrEmpty(FirstName)
			       && !string.IsNullOrEmpty(LastName);*/
		}

		public async void Register() {
			try {
				RegisterInfo info = new RegisterInfo(FirstName, LastName, Username, Password, DayOfBirth, Gender);
				await _authenticator.Register(info);
				OpenLoginCommand.Execute(null);
			}
			catch (UserNotFoundException e) {

			}
			catch (InvalidPasswordException e) {
				
			}
		}
		
	}
}
using System;
using System.Threading.Tasks;
using UI.Models;
using UI.Network;
using UI.Network.Packets;
using UI.Network.Packets.Login;
using UI.Network.Packets.Register;
using UI.Services.Exceptions;
using UI.Utils;

namespace UI.Services.Common {
	public class Authenticator : IAuthenticator {
		public IAppSession CurrentSession { get; }
		public bool IsLoggedIn { get; }
		public event Action StateChanged;

		private readonly ChatConnection _connection;

		public Authenticator(ChatConnection connection, IAppSession currentSession) {
			_connection = connection;
			CurrentSession = currentSession;
		}

		public async Task Register(RegisterInfo info) {
			RegisterData request = new RegisterData();
			request.Email = info.Username;
			request.PassHashed = HashUtils.MD5(info.Password);
			request.FirstName = info.Firstname;
			request.LastName = info.Lastname;
			request.DoB = info.DayOfBirth;
			request.Gender = info.Gender;
			RegisterResult result = await _connection.Send<RegisterResult>(request);
			int code = result.StatusCode;
			if (code != 200) {
				//TODO
				switch (code) {
					case 404:
					case 401:
                        throw new AlreadyExistsUserException(request.Email);
					case 403:
						//msgs = new [] { "Your account got banned", "Please contact the admin to get more information" };
						break;
					default:
						//msgs = new [] { "Unhandle status code " + statusCode };
						break;
				}

				return;
			}
		}

		public async Task Login(LoginInfo info) {
			LoginData request = new LoginData();
			request.Username = info.Username;
			request.Passhash = HashUtils.MD5(info.Password);
			LoginResult result = await _connection.Send<LoginResult>(request);
			int code = result.StatusCode;
			if (code != 200) {
				switch (code)
				{
					case 404:
						throw new UserNotFoundException(info.Username);
					case 401:
						throw new InvalidPasswordException(info.Username, info.Password);
					case 403:
						//TODO Gor banned exception
						break;
					default:
						//TODO Unhandle status code
						break;
				}

				return;
			}
			CurrentSession.SessionKey = request.Passhash;
		}

		public void Logout() {
			
		}
	}
}
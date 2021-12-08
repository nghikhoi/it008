using System;
using System.Threading.Tasks;
using UI.Models;

namespace UI.Services {
	public interface IAuthenticator {
		IAppSession CurrentSession { get; }
		bool IsLoggedIn { get; }

		event Action StateChanged;

		/// <summary>
		/// Register a new user.
		/// </summary>
		/// <param name="email">The user's email.</param>
		/// <param name="username">The user's name.</param>
		/// <param name="password">The user's password.</param>
		/// <param name="confirmPassword">The user's confirmed password.</param>
		/// <returns>The result of the registration.</returns>
		/// <exception cref="Exception">Thrown if the registration fails.</exception>
		Task Register(RegisterInfo info);

		/// <summary>
		/// Login to the application.
		/// </summary>
		/// <param name="username">The user's name.</param>
		/// <param name="password">The user's password.</param>
		/// <exception cref="UserNotFoundException">Thrown if the user does not exist.</exception>
		/// <exception cref="InvalidPasswordException">Thrown if the password is invalid.</exception>
		/// <exception cref="Exception">Thrown if the login fails.</exception>
		Task Login(LoginInfo info);

		void Logout();
	}
	
}
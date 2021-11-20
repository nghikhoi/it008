namespace UI.Models {

	public class LoginRespondeCode {

		public static readonly int LOGIN_SUCCESS = 200;
		public static readonly int INVALID_USERNAME_PASSWORD = 404;
		public static readonly int INVALID_USERNAME_PASSWORD_2 = 401;
		public static readonly int BANNED_ACCOUNT = 403;

	}
	
	public class LoginInfo {
		
		public string Username { get; private set; }
		public string Password { get; private set; }

		public LoginInfo(string username, string password) {
			Username = username;
			Password = password;
		}
		
	}
}
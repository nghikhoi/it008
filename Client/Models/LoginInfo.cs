namespace UI.Models {
	public class LoginInfo {
		
		public string Username { get; private set; }
		public string Password { get; private set; }

		public LoginInfo(string username, string password) {
			Username = username;
			Password = password;
		}
		
	}
}
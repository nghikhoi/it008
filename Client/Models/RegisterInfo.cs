using System;

namespace UI.Models {
	
	public class RegisterRespondeCode {

		public static readonly int REGISTER_SUCCESS = 200;
		public static readonly int EXCEPTION = 404;
		public static readonly int ALREADY_EXISTS = 409;

	}
	
	public class RegisterInfo {
		public string Firstname { get; private set; }
		public string Lastname { get; private set; }
		public string Username { get; private set; }
		public string Password { get; private set; }
		public DateTime DayOfBirth { get; private set; }
		public Gender Gender { get; private set; }

		public RegisterInfo(string firstname, string lastname, string username, string password, DateTime dayOfBirth, Gender gender) {
			Firstname = firstname;
			Lastname = lastname;
			Username = username;
			Password = password;
			DayOfBirth = dayOfBirth;
			Gender = gender;
		}
		
	}
}
using System;

namespace UI.Models {
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
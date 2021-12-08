using UI.Models;

namespace UI.Services {
	public interface IUserProfileHolder {
		
		UserProfile UserProfile { get; set; }

		void LoadProfile();

		void UpdateUserProfile(UserProfile userProfile);

		void ChangePassword(string oldPassword, string newPassword);

	}
}
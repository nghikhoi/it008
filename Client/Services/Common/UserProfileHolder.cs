using System.Threading.Tasks;
using UI.Models;
using UI.Network;
using UI.Network.Packets;
using UI.Network.Packets.AfterLoginRequest.Profile;
using UI.Network.RestAPI;
using UI.Services.Exceptions;

namespace UI.Services.Common {
	public class UserProfileHolder : IUserProfileHolder {

		private IAppSession _appSession;
		private ChatConnection _chatConnection;

		public UserProfileHolder(IAppSession appSession, ChatConnection chatConnection) {
			_appSession = appSession;
			_chatConnection = chatConnection;
		}
		
		public UserProfile UserProfile { get; set; }
		
		public void LoadProfile() {
			GetSelfProfile getSelfProfile = new GetSelfProfile();
			DataAPI.getData<GetSelfProfileResult>(getSelfProfile, result => {
				UserProfile = result.Profile;
			});
		}

		public void UpdateUserProfile(UserProfile userProfile) {
			UpdateSelfProfile request = new UpdateSelfProfile();
			request.UserProfile = userProfile;
			DataAPI.getData<GetSelfProfileResult>(request, result => {
				UserProfile = result.Profile;
			});
		}

		public void ChangePassword(string oldPassword, string newPassword) {
			ModifyPassword request = new ModifyPassword();
			request.OldPassword = oldPassword;
			request.NewPassword = newPassword;
			bool status = false;
			DataAPI.getData<ModifyPasswordResult>(request, result => {
				status = result.Result;
			});
			if (!status) {
				throw new WrongPasswordException(oldPassword, newPassword);
			}
		}
	}
}
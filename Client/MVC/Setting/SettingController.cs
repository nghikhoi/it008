using UI.Models;

namespace UI.MVC {
	
	public partial class SettingPageController : IController {

		private SettingWindow view;

		public SettingPageController(SettingWindow view) {
			this.view = view;
		}

		public void updateProfile(UserProfile profile) {
			ChatModel.Instance.Profile = profile;
			view.updateProfile(profile);
		}

	}

}
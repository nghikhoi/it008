using System;
using System.Windows.Media;
using UI.Models;
using UI.MVC;

namespace UI.MVC {
	
	public partial class SettingPageController : IController {

		private SettingWindow view;

		public SettingPageController(SettingWindow view) {
			this.view = view;
		}

		public void updateProfile(UserProfile profile) {
			//TODO cập nhật thông tin lên giao diện
		}

	}

}
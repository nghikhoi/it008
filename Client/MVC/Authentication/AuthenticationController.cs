using System;
using System.Windows.Media;
using UI.Models;
using UI.MVC;

namespace UI.MVC {
	
	public partial class AuthenticationController : IController {

		private HomeWindow view;

		public AuthenticationController(HomeWindow view) {
			this.view = view;
		}

		public void LoginResponde(int statusCode) {
			
		}

		public void RegisterResponse(int statusCode) {
			
		}

	}

}
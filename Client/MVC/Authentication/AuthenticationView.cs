namespace UI.MVC {
	
	public class AuthenticationView : IView{
		
		private WindowLogIn loginView;
		private WindowRegister registerView;

		public AuthenticationView(WindowLogIn loginView, WindowRegister registerView) {
			this.loginView = loginView;
			this.registerView = registerView;
		}
		
	}
	
}
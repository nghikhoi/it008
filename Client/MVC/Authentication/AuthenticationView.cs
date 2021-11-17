using MaterialDesignThemes.Wpf;
using UI.CustomControls;
using UI.Utils;

namespace UI.MVC {
	
	public class AuthenticationView : IView{
		
		private WindowLogIn loginView;
		private WindowRegister registerView;

		public AuthenticationView(WindowLogIn loginView, WindowRegister registerView) {
			this.loginView = loginView;
			this.registerView = registerView;
		}

        //public void LoginResponse(int code)
        //{
        //    DialogHost.CloseDialogCommand.Execute(null, null);
        //    if (code == 200)
        //    {
        //        //
        //    }
        //    else
        //    {
        //        AnnouncementDialog dialog = null;
        //        switch (code)
        //        {
        //            case 404:
        //                dialog = new AnnouncementDialog("Invalid username or password", "Check your info and try again");
        //                break;
        //            case 401:
        //                dialog = new AnnouncementDialog("Invalid username or password", "Check your info and try again");
        //                break;
        //            case 403:
        //                dialog = new AnnouncementDialog("Your account got banned", "Please contact the admin to get more information");
        //                break;
        //        }
        //        DialogHost.Show(dialog);
        //    }
        //}

    }
	
}
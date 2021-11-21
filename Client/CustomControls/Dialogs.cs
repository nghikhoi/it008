using MaterialDesignThemes.Wpf;

namespace UI.CustomControls {
	
	public class Dialogs {

		public static void openAnnouncement(params string[] msgs) {
			AnnouncementDialog dialog = new AnnouncementDialog(msgs);
			DialogHost.Show(dialog);
		}
		
	}
	
}
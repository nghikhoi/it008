using MaterialDesignThemes.Wpf;

namespace UI.Services.Common {
	public class Dialog : IDialog {
		public void ShowError(string message) {
			DialogHost.Show(null);
		}

		public void ShowInfo(string message) {
			throw new System.NotImplementedException();
		}
	}
}
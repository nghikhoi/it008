using System;
using System.Windows;
using UI.ViewModels;

namespace UI.Services.Common {
	public class MainWindowNavigator<TWindow> : INavigator where TWindow : Window {

		private readonly Func<TWindow> _windowProvider;

		public MainWindowNavigator(Func<TWindow> _windowProvider) {
			this._windowProvider = _windowProvider;
		}

		public void Navigate() {
			Window oldWindow = App.Current.MainWindow;
			Window newWindow = _windowProvider.Invoke();
			newWindow.Show();
			App.Current.MainWindow = newWindow;
			oldWindow?.Close();
		}
		
	}
}
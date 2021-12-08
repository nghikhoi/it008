using UI.ViewModels;

namespace UI.Services.Common {
	public class ViewStateDelegateNavigator<VM> : INavigator where VM : ViewModelBase {
		
		private readonly IViewState _viewstate;
		private readonly ViewModelCreator<VM> _createViewModel;

		public ViewStateDelegateNavigator(IViewState viewstate, ViewModelCreator<VM> createViewModel)
		{
			_viewstate = viewstate;
			_createViewModel = createViewModel;
		}

		public void Navigate() {
			_viewstate.CurrentViewModel = _createViewModel();
		}
		
	}
}
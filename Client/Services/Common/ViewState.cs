using System;
using UI.ViewModels;

namespace UI.Services.Common
{
    public class ViewState : IViewState
    {
        private ViewModelBase _currentViewModel;
        public ViewModelBase CurrentViewModel
        {
            get {
                return _currentViewModel;
            } set {
                _currentViewModel?.Dispose();
                _currentViewModel = value;
                StateChanged?.Invoke();
            }
        }

        public event Action StateChanged;

    }
}

using System;
using UI.Services;

namespace UI.ViewModels.Authentication {
    public class AuthenticationViewModel : InitializableViewModel, IViewState {

        private ViewModelBase _currentViewModel;
        public ViewModelBase CurrentViewModel {
            get => _currentViewModel;
            set {
                _currentViewModel = value;
                OnPropertyChanged(nameof(CurrentViewModel));
                StateChanged?.Invoke();
            }
        }

        public event Action StateChanged;

        public AuthenticationViewModel() {
            
        }

        protected override void Initialize(object parameter = null) {
            
        }
        
    }
}
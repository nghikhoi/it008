using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UI.Command;
using UI.Services;
using UI.ViewModels.Authentication;

namespace UI.ViewModels {
    public class MainViewModel : ViewModelBase, IViewState {
        public ViewModelBase CurrentViewModel {
            get => _viewState.CurrentViewModel;
            set {
                _viewState.CurrentViewModel = value;
                OnPropertyChanged(nameof(CurrentViewModel));
            }
        }

        public event Action StateChanged {
            add => _viewState.StateChanged += value;
            remove => _viewState.StateChanged -= value;
        }

        private readonly IViewState _viewState;
        private readonly IViewModelFactory _factory;

        public MainViewModel(IViewState viewState, IViewModelFactory factory) {
            _viewState = viewState;
            _factory = factory;
            UpdateCurrentViewModelCommand<HomeViewModel> command = new UpdateCurrentViewModelCommand<HomeViewModel>(_viewState, factory);
            command.Execute();
            /*UpdateCurrentViewModelCommand<AuthenticationViewModel> command = new UpdateCurrentViewModelCommand<AuthenticationViewModel>(_viewState, factory);
            command.Execute();

            AuthenticationViewModel authenticationViewModel = (AuthenticationViewModel) CurrentViewModel;
            UpdateCurrentViewModelCommand<LoginViewModel> loginCommand = new UpdateCurrentViewModelCommand<LoginViewModel>(authenticationViewModel, factory);
            loginCommand.Execute();*/
        }
    }
}
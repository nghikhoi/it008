using System;
using System.Windows.Input;
using UI.Services;
using UI.ViewModels;

namespace UI.Command
{
    public class UpdateCurrentViewModelCommand<TViewModel> : ICommand where TViewModel : ViewModelBase
    {
        public event EventHandler CanExecuteChanged;

        private readonly IViewState _viewState;
        private readonly IViewModelFactory _viewModelFactory;

        public UpdateCurrentViewModelCommand(IViewState viewState, IViewModelFactory viewModelFactory)
        {
            _viewState = viewState;
            _viewModelFactory = viewModelFactory;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter = null) {
            _viewState.CurrentViewModel = _viewModelFactory.Create<TViewModel>();
        }
    }
}
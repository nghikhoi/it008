using System;
using UI.ViewModels;

namespace UI.Services {
    public enum ViewType
    {
        Login,
        Home,
        Portfolio,
        Buy,
        Sell
    }

    public interface IViewState
    {
        ViewModelBase CurrentViewModel { get; set; }
        event Action StateChanged;
    }
}

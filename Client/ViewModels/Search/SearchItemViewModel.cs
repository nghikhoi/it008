using System;
using System.Windows.Input;
using UI.Command;
using UI.Models;

namespace UI.ViewModels.Search {
    public class SearchItemViewModel : ViewModelBase {

        #region Properties

        private UserShortInfo _info;
        public UserShortInfo Info {
            get => _info;
            set {
                _info = value;
                OnPropertyChanged(nameof(Info));
                OnPropertyChanged(nameof(UserId));
                OnPropertyChanged(nameof(NickName));
            }
        }

        public string UserId
        {
            get => Info.ID;
        }

        public string NickName
        {
            get => Info.FirstName + " " + Info.LastName;
            set { }
        }

        #endregion

        public event Action<SearchItemViewModel> SelectEvent;
        public event Action<SearchItemViewModel> DiscardEvent;

        #region Command

        public ICommand SelectCommand { get; private set; }
        public ICommand DiscardCommand { get; private set; }

        #endregion

        public SearchItemViewModel() : base()
        {
            SelectCommand = new RelayCommand<object>(null, o => SelectEvent?.Invoke(this));
            DiscardCommand = new RelayCommand<object>(null, o => DiscardEvent?.Invoke(this));

        }

    }
}
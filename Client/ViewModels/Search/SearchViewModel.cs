using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using UI.Command;
using UI.Models;
using UI.Utils;

namespace UI.ViewModels.Search {
    public class SearchViewModel : ViewModelBase {

        #region Properties

        private string _searchingString;
        public string SearchingString {
            get => _searchingString;
            set {
                _searchingString = value;
                OnPropertyChanged(nameof(SearchingString));
            }
        }

        private ObservableCollection<SearchItemViewModel> _searchList;
        public ObservableCollection<SearchItemViewModel> SearchList { get => _searchList; set => _searchList = value; }

        private ObservableCollection<SearchItemViewModel> _selectedList;
        public ObservableCollection<SearchItemViewModel> SelectedList { get => _selectedList; set => _selectedList = value; }
        
        private readonly IModelContext _model;

        public HashSet<Guid> Blacklist { get; set; } = new HashSet<Guid>();

        #endregion

        public event Action<List<UserShortInfo>> AcceptEvent;

        #region Command

        public ICommand SearchCommand { get; private set; }
        public ICommand AcceptCommand { get; private set; }

        #endregion

        public SearchViewModel(IModelContext model) : base() {
            _searchList = new ObservableCollection<SearchItemViewModel>();
            _selectedList = new ObservableCollection<SearchItemViewModel>();
            this._model = model;
            
            SearchCommand = new RelayCommand<object>(null, o => SearchAction(SearchingString));
            AcceptCommand = new RelayCommand<object>(null,
                o => AcceptEvent?.Invoke(SelectedList.Select(m => m.Info).ToList()));
        }
        
        public void SearchAction(string s = null)
        {
            if (!FastCodeUtils.NotEmptyStrings(s))
            {
                SearchList.Clear();
                return;
            }
            ShowSearchResult(_model.Search(s));
        }

        private void Select(SearchItemViewModel model) {
            SelectedList.Add(model);
            SearchAction(SearchingString);
        }

        private void Discard(SearchItemViewModel model)
        {
            SelectedList.Remove(model);
            SearchAction(SearchingString);
        }

        public void ShowSearchResult(List<UserShortInfo> list) {
            SearchList.Clear();
            foreach (var info in list) {
                if (SelectedList.Any(model => model.Info.ID == info.ID)) continue;
                if (Blacklist.Any(id => id.ToString() == info.ID)) continue;
                SearchItemViewModel m = new SearchItemViewModel()
                {
                    Info = info
                };
                m.SelectEvent += Select;
                m.DiscardEvent += Discard;
                SearchList.Add(m);
            }
        }

    }
}
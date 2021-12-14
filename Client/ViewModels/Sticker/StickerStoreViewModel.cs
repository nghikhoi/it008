using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using UI.Command;
using UI.Models.Message;
using UI.Services;

namespace UI.ViewModels {
    public class StickerStoreViewModel : ViewModelBase
    {

        public ObservableCollection<StickerItemStoreViewModel> Items
        {
            get;
            private set;
        }
        public ObservableCollection<StickerItemStoreViewModel> OriginItems {
            get;
            private set;
        }


        private readonly IViewModelFactory _factory;
        public ICommand LoadMoreCommand { get; private set; }

        public StickerStoreViewModel(IViewModelFactory factory)
        {
            Items = new ObservableCollection<StickerItemStoreViewModel>();
            OriginItems = new ObservableCollection<StickerItemStoreViewModel>();
            _factory = factory;

            LoadMoreCommand = new RelayCommand<object>(null, o => LoadMore());
        }

        private void LoadMore(int quantity = 5)
        {
            Console.WriteLine("Load more " + quantity);
            int root = Items.Count;
            for (int i = 0; i < root + quantity && i < OriginItems.Count; i++)
            {
                Items.Add(OriginItems[i]);
            }
        }

        public void AddCategory(StickerCategory category)
        {
            if (category == null)
                return;
            StickerItemStoreViewModel viewModel = _factory.Create<StickerItemStoreViewModel>();
            viewModel.Category = category;
            OriginItems.Add(viewModel);
        }

    }
}

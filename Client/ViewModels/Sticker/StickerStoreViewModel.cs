using System.Collections.ObjectModel;
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


        private readonly IViewModelFactory _factory;

        public StickerStoreViewModel(IViewModelFactory factory)
        {
            Items = new ObservableCollection<StickerItemStoreViewModel>();
            _factory = factory;
        }

        public void AddCategory(StickerCategory category)
        {
            if (category == null)
                return;
            StickerItemStoreViewModel viewModel = _factory.Create<StickerItemStoreViewModel>();
            viewModel.Category = category;
            Items.Add(viewModel);
        }

    }
}

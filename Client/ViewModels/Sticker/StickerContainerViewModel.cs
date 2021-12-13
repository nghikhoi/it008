using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using UI.Components;
using UI.Models.Message;
using UI.Network.Packets.AfterLoginRequest.Sticker;
using UI.Network.RestAPI;
using UI.Services;
using UI.Utils;

namespace UI.ViewModels {
    public class StickerContainerViewModel : InitializableViewModel
    {

        private StickerStoreViewModel _store;
        public StickerStoreViewModel Store
        {
            get => _store;
            set {
                _store = value;
                OnPropertyChanged(nameof(Store));
            }
        }

        private StickerRecentTabViewModel _recentTab;
        private StickerRecentTabViewModel RecentTab
        {
            get => _recentTab;
            set
            {
                _recentTab = value;
                OnPropertyChanged(nameof(RecentTab));
            }
        }

        public ObservableCollection<object> Tabs { get; private set; }

        private readonly IViewModelFactory _factory;
        public event Action<Sticker> OnStickerClick;

        public StickerContainerViewModel(IViewModelFactory factory)
        {
            _factory = factory;

            Tabs = new ObservableCollection<object>();
            Store = _factory.Create<StickerStoreViewModel>();
            RecentTab = _factory.Create<StickerRecentTabViewModel>();
            Tabs.Add(Store);
            Tabs.Add(RecentTab);

            InitializeCommand.Execute(null);
        }

        protected override void Initialize(object parameter = null)
        {
            Sticker.Load(InitStickers);
        }

        private void InitStickers()
        {
            DataAPI.getData<GetBoughtStickerPacksRequest, GetBoughtStickerPacksResponse>(result =>
            {
                foreach (KeyValuePair<int, StickerCategory> pair in Sticker.LoadedCategories)
                {
                    int key = pair.Key;
                    StickerCategory category = pair.Value;
                    bool bought = result.BoughtStickerPacks.Remove(key);
                    if (bought) AddOwnedCategory(category);
                    else Store.AddCategory(category);
                }
            });
            DataAPI.getData<GetNearestSickerRequest, GetNearestSickerResponse>(result =>
            {
                result.NearestSticker.ForEach(k => RecentTab.AddSticker(Sticker.LoadedStickers[k]));
            });
        }

        public void AddOwnedCategory(StickerCategory category)
        {
            StickerOwnedTabViewModel viewModel = _factory.Create<StickerOwnedTabViewModel>();
            viewModel.Category = category;
            Tabs.Add(viewModel);
        }

    }
}

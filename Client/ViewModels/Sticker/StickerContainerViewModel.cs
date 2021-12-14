using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using UI.Components;
using UI.Models.Message;
using UI.Network;
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

        public StickerContainerViewModel(IViewModelFactory factory, PacketRespondeListener listener)
        {
            _factory = factory;

            Tabs = new ObservableCollection<object>();
            Store = _factory.Create<StickerStoreViewModel>();
            RecentTab = _factory.Create<StickerRecentTabViewModel>();
            Tabs.Add(Store);
            //Tabs.Add(RecentTab);
            listener.BuyStickerResponseEvent += (s, r) => App.Current.Dispatcher.Invoke(() => OnBought(r.CateID));

            InitializeCommand.Execute(null);
        }

        private void Invoke(Sticker sticker)
        {
            OnStickerClick?.Invoke(sticker);
        }

        protected override void Initialize(object parameter = null)
        {
            Sticker.Load(InitStickers);
        }

        private void OnBought(int cateId)
        {
            for (var i = Store.Items.Count - 1; i >= 0; i--)
            {
                if (cateId == Store.Items[i].Category.ID)
                {
                    Store.Items.RemoveAt(i);
                    break;
                }
            }
            for (var i = Store.OriginItems.Count - 1; i >= 0; i--) {
                if (cateId == Store.OriginItems[i].Category.ID) {
                    Store.OriginItems.RemoveAt(i);
                    break;
                }
            }

            StickerCategory cate = Sticker.LoadedCategories[cateId];
            if (cate != null)
                AddOwnedCategory(cate);
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
                Store.LoadMoreCommand.Execute(null);
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
            viewModel.OnStickerClick += Invoke;
            Tabs.Add(viewModel);
        }

    }
}

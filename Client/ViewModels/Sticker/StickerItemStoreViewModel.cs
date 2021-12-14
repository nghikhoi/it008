using System.Windows.Input;
using UI.Command;
using UI.Models.Message;
using UI.Network;
using UI.Network.Packets.AfterLoginRequest.Sticker;

namespace UI.ViewModels {
    public class StickerItemStoreViewModel : ViewModelBase {

        private StickerCategory _category;
        public StickerCategory Category {
            get => _category;
            set {
                _category = value;
                OnPropertyChanged(nameof(Category));
            }
        }

        public string ThumbnailURL => Category.ThumbImg;
        public string IconURL => Category.IconURL;
        public string Name => Category.Name;

        public ICommand BuyCommand { get; private set; }
        private readonly ChatConnection _connection;

        public StickerItemStoreViewModel(ChatConnection connection)
        {
            _connection = connection;
            BuyCommand = new RelayCommand<object>(null, o => BuyCategory());
        }

        protected void BuyCategory()
        {
            BuyStickerCategoryRequest request = new BuyStickerCategoryRequest();
            request.CateID = Category.ID;
            _connection.Send(request);
        }

    }
}

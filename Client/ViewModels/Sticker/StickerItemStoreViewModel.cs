using UI.Models.Message;

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

    }
}

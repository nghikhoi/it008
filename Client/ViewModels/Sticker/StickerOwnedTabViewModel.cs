using UI.Models.Message;

namespace UI.ViewModels {
    public class StickerOwnedTabViewModel : StickerTabViewModel {

        private StickerCategory _category;
        public StickerCategory Category {
            get => _category;
            set {
                _category = value;
                UpdateSticker();
                OnPropertyChanged(nameof(Category));
            }
        }

        private void UpdateSticker()
        {
            Category.Stickers.ForEach(AddSticker);
        }

    }
}

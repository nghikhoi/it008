using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using UI.Command;
using UI.Models.Message;

namespace UI.ViewModels {
    public class StickerTabViewModel : ViewModelBase
    {
        public event Action<Sticker> OnStickerClick;
        public ObservableCollection<StickerViewModel> Stickers
        {
            get;
            private set;
        }

        public StickerTabViewModel()
        {
            Stickers = new ObservableCollection<StickerViewModel>();
        }

        private void Invoke(Sticker sticker)
        {
            OnStickerClick?.Invoke(sticker);
        }

        public void AddSticker(Sticker sticker)
        {
            if (sticker == null)
                return;
            StickerViewModel vm = new StickerViewModel();
            vm.Sticker = sticker;
            vm.OnStickerClick += Invoke;
            Stickers.Add(vm);
        }

    }

    public class StickerViewModel : ViewModelBase {
        private Sticker _sticker;
        public Sticker Sticker {
            get => _sticker;
            set {
                _sticker = value;
                OnPropertyChanged(nameof(Sticker));
                OnPropertyChanged(nameof(StickerURL));
            }
        }

        public string StickerURL {
            get => Sticker.StickerURL;
        }
        public event Action<Sticker> OnStickerClick;
        public ICommand RaiseClickCommand { get; set; }

        public StickerViewModel()
        {
            RaiseClickCommand = new RelayCommand<object>(null, o => OnStickerClick?.Invoke(Sticker));
        }
    }
}

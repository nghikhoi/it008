using System.Collections.ObjectModel;

namespace UI.ViewModels {
    public class DownloadManagerViewModel : ViewModelBase
    {

        public static DownloadManagerViewModel Instance { get; private set; }

        public ObservableCollection<DownloadItemViewModel> Items { get; } =
            new ObservableCollection<DownloadItemViewModel>();

        public DownloadManagerViewModel()
        {
            Instance = this;
        }

        public void Add(DownloadItemViewModel item)
        {
            Items.Add(item);
        }

    }
}

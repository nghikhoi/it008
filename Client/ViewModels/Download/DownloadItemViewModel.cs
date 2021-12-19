using System;
using System.Diagnostics;
using System.Net;
using System.Windows.Input;
using UI.Command;
using UI.Network.RestAPI;
using UI.Utils;

namespace UI.ViewModels {
    public class DownloadItemViewModel : ViewModelBase
    {

        private double _speed; //kb/s
        public double Speed
        {
            get => _speed;
            set
            {
                _speed = value;
                OnPropertyChanged(nameof(Speed));
                OnPropertyChanged(nameof(Status));
            }
        }

        public string Status => Completed ? "Download Completed" : $"{Speed:00} KB/s";

        private int _progress;
        public int Progress
        {
            get => _progress;
            set
            {
                _progress = value;
                OnPropertyChanged(nameof(Progress));
            }
        }

        private string _fileName;
        public string FileName
        {
            get => _fileName;
            set
            {
                _fileName = value;
                OnPropertyChanged(nameof(FileName));
            }
        }

        public event Action<DownloadItemViewModel> CompleteEvent;

        //0: Media
        //1: Attachment
        public int FileType { get; set; } = 0;
        public string SavePath { get; set; }
        public string FileId { get; set; }
        public string ConversationId { get; set; }

        private double StartTime { get; set; }
        private double TotalBytesReceived { get; set; }
        private bool Completed = false;

        public ICommand OpenExplorerCommand { get; private set; }

        public DownloadItemViewModel()
        {
            OpenExplorerCommand = new RelayCommand<object>(null, o =>
            {
                if (FastCodeUtils.NotEmptyStrings(SavePath))
                    Process.Start(System.IO.Directory.GetParent(SavePath).FullName);
            });
        }

        protected void InvokeComplete()
        {
            CompleteEvent?.Invoke(this);
            Completed = true;
            OnPropertyChanged(nameof(Status));
        }

        public void Start()
        {
            StartTime = DateUtils.CurrentTimeMillis();
            if (FileType == 0)
            {
                FileAPI.DownloadMedia(ConversationId, FileId, SavePath, ProgressUpdate, (sender, args) =>
                {
                    App.Current.Dispatcher.Invoke(InvokeComplete);
                }, error => { });
            }
            else
            {
                FileAPI.DownloadAttachment(ConversationId, FileId, SavePath, ProgressUpdate, (sender, args) => {
                    App.Current.Dispatcher.Invoke(InvokeComplete);
                }, error => { });
            }
        }

        private void ProgressUpdate(object sender, DownloadProgressChangedEventArgs args)
        {
            UpdateProgress(args.ProgressPercentage);
            UpdateSpeed(args.BytesReceived);
        }

        private void UpdateProgress(int value)
        {
            App.Current.Dispatcher.Invoke(() => Progress = value);
        }

        private void UpdateSpeed(long BytesReceived)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                double time = (DateUtils.CurrentTimeMillis() - StartTime) / 1000;
                TotalBytesReceived += BytesReceived;
                Speed = (int) (TotalBytesReceived / time);
            });
        }

    }
}

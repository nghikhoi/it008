using System;
using System.Net;
using UI.Network.RestAPI;

namespace UI.ViewModels {
    public class DownloadMediaItemViewModel : DownloadItemViewModel
    {
        public string StreamURL { get; set; }

        public long StartTime { get; set; }

        public void Start()
        {
            StartTime = new DateTime().Second;
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
            App.Current.Dispatcher.Invoke(() => Speed = (int) (BytesReceived * 1000 / (new DateTime().Second - StartTime)));
        }

    }
}

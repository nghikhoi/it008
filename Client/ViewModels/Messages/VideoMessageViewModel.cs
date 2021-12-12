using System;
using UI.Models;
using UI.Models.Message;
using UI.Network.RestAPI;
using UI.Services;

namespace UI.ViewModels {
	public class VideoMessageViewModel : MessageViewModel, MediaViewModel {
		public new VideoMessage Message {
			get => (VideoMessage) base.Message;
			set {
				base.Message = value;
			}
		}

		private MediaInfo _mediaInfo;
		public MediaInfo MediaInfo {
			get => _mediaInfo;
			set {
				_mediaInfo = value;
				OnPropertyChanged(nameof(MediaInfo));
			}
		}

		private void UpdateMediaInfo() {
			string fileId = Message.FileID;
			string fileName = Message.FileName;
			String thumbUrl = StreamAPI.GetMediaThumbnailURL(fileId, ConversationId);
			String streamUrl = StreamAPI.GetMediaURL(fileId, ConversationId);
			MediaInfo media = new MediaInfo(thumbUrl, streamUrl, fileName, fileId);
			MediaInfo = media;
		}

		public VideoMessageViewModel(IAppSession appSession) : base(appSession) {
			PropertyChanged += (sender, args) => {
				if (string.CompareOrdinal(args.PropertyName, nameof(Message)) == 0)
					UpdateMediaInfo();
			};
		}
	}
}
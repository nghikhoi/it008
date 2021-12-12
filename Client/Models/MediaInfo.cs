using System.Collections.Generic;

namespace UI.Models {
	
	public class MediaInfo {
		
		public string ThumbURL { get; set; }
		public string StreamURL { get; set; }
		public string FileName { get; set; }
		public string FileID { get; set; }

		public MediaInfo(string thumbUrl, string streamUrl, string fileName, string fileID)
		{
			ThumbURL = thumbUrl;
			StreamURL = streamUrl;
			FileName = fileName;
			FileID = fileID;
		}

		public bool IsVideo() {
			return IsVideoFileName(FileName);
		}

		private readonly static List<string> SupportedExtensions = new List<string>() {
			".mp4", ".wmv" //TODO: Update more extensions
		};

		public static bool IsVideoFileName(string fileName) {
			return SupportedExtensions.Contains(System.IO.Path.GetExtension(fileName).ToLower());
		}
		
	}
	
}
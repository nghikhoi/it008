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
		
	}
	
}
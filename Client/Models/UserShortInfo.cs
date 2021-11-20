using System;

namespace UI.Models {
	
	public class UserShortInfo {
		
		public string ID { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string LastMessage { get; set; }
		public string ConversationID { get; set; }
		public int PreviewCode { get; set; }
		public bool IsOnline { get; set; }
		public DateTime LastLogout { get; set; }
		public int Relationship { get; set; }
		public long LastActive { get; set; }
		
	}
	
}
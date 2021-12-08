using System.ComponentModel;

namespace UI.Services.Common {
	
	public class AppSession : IAppSession {

		private string _sessionKey;
		public string SessionKey {
			get => _sessionKey;
			set {
				_sessionKey = value;
				SessionKeyChanged?.Invoke(this, new PropertyChangedEventArgs("SessionKey"));
			}
		}
		public event PropertyChangedEventHandler SessionKeyChanged;
		private string _sessionId;
		public string SessionID {
			get => _sessionId;
			set {
				_sessionId = value;
				SessionIDChanged?.Invoke(this, new PropertyChangedEventArgs("SessionID"));
			}
		}
		public event PropertyChangedEventHandler SessionIDChanged;
		
	}
}
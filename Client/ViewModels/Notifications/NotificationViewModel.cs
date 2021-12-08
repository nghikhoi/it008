using System.Collections.ObjectModel;
using UI.Models.Message;
using UI.Utils;

namespace UI.ViewModels.Notifications {
	public abstract class NotificationViewModel : ViewModelBase {

		#region Properties

		private NotificationInfo _info;
		public NotificationInfo Info {
			get => _info;
			set {
				_info = value;
				OnPropertyChanged(nameof(Info));
			}
		}
		
		#endregion
		
		protected NotificationViewModel() {
		}

	}
}
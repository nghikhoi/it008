using UI.Models.Notification;
using UI.Utils;

namespace UI.ViewModels {
	public abstract class NotificationViewModel : ViewModelBase {

		#region Properties

		private AbstractNotification _info;
		public AbstractNotification Info {
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
using UI.Utils;

namespace UI.ViewModels {
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
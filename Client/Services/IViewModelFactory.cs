using UI.Annotations;
using UI.ViewModels;

namespace UI.Services {
	public interface IViewModelFactory {

		[NotNull]
		TViewModel Create<TViewModel>() where TViewModel : ViewModelBase;

	}
}
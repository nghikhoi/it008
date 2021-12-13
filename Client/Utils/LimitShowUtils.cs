using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace UI.Utils {
	public class LimitShowUtils {

		public static NotifyCollectionChangedEventHandler CreateHandler<T>(ObservableCollection<T> source,
			ObservableCollection<T> listener, int from, int length) {
			return (sender, args) => {
				listener.Clear();
				for (int i = from; i < from + length && i < source.Count; i++)
					listener.Add(source[i]);
				//TODO: Optimize
			};
		}
		
	}
}
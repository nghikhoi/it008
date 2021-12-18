using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace UI.Utils {
	public static class ObserveUtils {

		public delegate void IDispose();

		public static NotifyCollectionChangedEventHandler CreateLimitObserve<T>(ObservableCollection<T> source,
			ObservableCollection<T> listener, int from, int length) {
			return (sender, args) => {
				listener.Clear();
				for (int i = from; i < from + length && i < source.Count; i++)
					listener.Add(source[i]);
				//TODO: Optimize
			};
		}

        public static IDispose CreateObserve<T, R>(this ObservableCollection<T> collection, ObservableCollection<R> source,
            Func<R, T> converter)
        {
            NotifyCollectionChangedEventHandler handler = (sender, args) =>
            {
                switch (args.Action)
                {
                    case NotifyCollectionChangedAction.Reset:
                    {
                        collection.Clear();
                        break;
                    }
                    case NotifyCollectionChangedAction.Add:
                    {
                        for (var i = args.NewStartingIndex; i < args.NewItems.Count + args.NewStartingIndex; i++)
                        {
                            T obj = converter.Invoke((R) args.NewItems[i + args.NewStartingIndex]);
                            if (i >= collection.Count)
                            {
                                collection.Add(obj);
                            }
                            else
                            {
                                collection.Insert(i, obj);
                            }
                        }
                        break;
                    }
                    case NotifyCollectionChangedAction.Move:
                    {
                        collection.Move(args.OldStartingIndex, args.NewStartingIndex);
                        break;
                    }
                    case NotifyCollectionChangedAction.Remove:
                    {
                        collection.RemoveAt(args.OldStartingIndex);
                        break;
                    }
                    case NotifyCollectionChangedAction.Replace:
                    {
                        //TODO
                        break;
                    }
                }
            };
            source.CollectionChanged += handler;
            return () => source.CollectionChanged -= handler;
        }
		
	}
}
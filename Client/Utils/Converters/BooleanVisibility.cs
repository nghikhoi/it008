using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace UI.Utils.Converters {
	public class BooleanVisibility : IValueConverter {
		public object Convert(object value, Type targetType, object isOpposite, CultureInfo culture) {
			bool source;
			if (value != null) {
				bool.TryParse(value.ToString(), out source);
			}
			else source = true;

			if (isOpposite != null) {
				bool temp;
				if (bool.TryParse(isOpposite.ToString(), out temp) && temp)
					source = !source;
			}
			return source ? Visibility.Visible : Visibility.Hidden;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
			throw new NotImplementedException();
		}
	}
}
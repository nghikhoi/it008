﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace UI.Utils.Converters {
	public class DataTypeConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
			return value?.GetType() ?? Binding.DoNothing;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
			throw new NotImplementedException();
		}
	}
}
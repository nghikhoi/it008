using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace UI.Utils.Converters {
    public class RatioConverter : MarkupExtension, IValueConverter {
        private static RatioConverter _instance;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Console.WriteLine(value + " : " + parameter);
            double ratio = NumberUtils.GetDouble(parameter.ToString());
            int intSource;
            if (int.TryParse(value.ToString(), out intSource))
            {
                intSource = (int) (intSource * ratio);
                Console.WriteLine(intSource + " - " + ratio);
                return intSource;
            }
            double doubleSource = NumberUtils.GetDouble(value.ToString());
            return doubleSource * ratio;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider) {
            return _instance ?? (_instance = new RatioConverter());
        }

    }
}

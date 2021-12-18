using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace UI.Utils.Converters
{
    class ProgressConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            
            if(value !=null && value is double)
            {
                double? temp = value as double?;
                if (temp == 100)
                    return "Completed.";
                else
                {
                    return temp.ToString() + "%";
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value != null)
            {
                var tmp = value as string;
                while (tmp.Contains("%"))
                {
                    tmp = tmp.Replace("%", "");
                }
                while(tmp.Contains(" "))
                {
                    tmp = tmp.Replace(" ", "");
                }
                if(double.TryParse(tmp,out var result))
                {
                    return result;
                }
            }
            return null;
        }
    }
}

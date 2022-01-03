using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Utils {
    public static class NumberUtils {

        public static double GetDouble(string value, double defaultValue = 0) {
            double result;
            // Try parsing in the current culture
            if (!double.TryParse(value.Replace(',', '.'), System.Globalization.NumberStyles.Any,
                    CultureInfo.InvariantCulture, out result)) {
                result = defaultValue;
            }
            return result;
        }

    }
}

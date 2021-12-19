using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Utils {
    public static class DateUtils {

        public static readonly DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static long CurrentTimeMillis() {
            return (long) (DateTime.UtcNow - epoch).TotalMilliseconds;
        }

    }
}

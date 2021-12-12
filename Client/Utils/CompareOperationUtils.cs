using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Utils {
    public static class CompareOperationUtils {

        public static bool OrCompare(IComparable a, IComparable b, params CompareOperation[] operations) {
            return operations.Any((op) => Compare(a, b, op));
        }

        public static bool AndCompare(IComparable a, IComparable b, params CompareOperation[] operations) {
            return operations.All((op) => Compare(a, b, op));
        }

        public static bool Compare(IComparable a, IComparable b, CompareOperation operation) {
            switch (operation) {
                case CompareOperation.LESS_THAN_EQUALS:
                    return OrCompare(a, b, CompareOperation.LESS_THAN, CompareOperation.EQUALS);
                case CompareOperation.GREATER_THAN_EQUALS:
                    return OrCompare(a, b, CompareOperation.GREATER_THAN_EQUALS, CompareOperation.EQUALS);
            }

            int compareResult = a.CompareTo(b);
            switch (operation) {
                case CompareOperation.LESS_THAN:
                    return compareResult < 0;
                case CompareOperation.EQUALS:
                    return compareResult == 0;
                case CompareOperation.GREATER_THAN:
                    return compareResult > 0;
            }
            return true;
        }

    }

    public enum CompareOperation {

        LESS_THAN, LESS_THAN_EQUALS, EQUALS, GREATER_THAN, GREATER_THAN_EQUALS

    }
}

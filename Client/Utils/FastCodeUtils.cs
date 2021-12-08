using System;

namespace UI.Utils {
	public static class FastCodeUtils {

		public static bool NotEmptyStrings(params string[] arr) => And(s => !string.IsNullOrEmpty(s) && !string.IsNullOrWhiteSpace(s), arr);

		public static bool And<T>(Func<T, bool> validate, params T[] arr) {
			foreach (var o in arr) {
				if (!validate.Invoke(o))
					return false;
			}
			return true;
		}
		
	}
}
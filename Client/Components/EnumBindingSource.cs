using System;
using System.Windows.Markup;

namespace UI.Components {

	public class EnumBindingSource : MarkupExtension {
		
		public Type EnumType { get; set; }

		public EnumBindingSource(Type enumType) {
			if (enumType == null || !enumType.IsEnum)
				throw new Exception("EnumType must not be null and of type Enum");
			EnumType = enumType;
		}

		public override object ProvideValue(IServiceProvider serviceProvider) {
			return Enum.GetValues(EnumType);
		}
		
	}
	
}
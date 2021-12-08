using System;
using System.Windows;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors;

namespace UI.Utils.Triggers {
	
	public class KeyDownTrigger : EventTriggerBase<object> {
		
		public static readonly DependencyProperty KeyProperty = DependencyProperty.Register(nameof(Key), typeof(Key), typeof(KeyDownTrigger)
			, new FrameworkPropertyMetadata(Key.Escape));
		
		public Key Key {
			get { return (Key) this.GetValue(KeyProperty); }
			set { this.SetValue(KeyProperty, value); }
		}
		
		protected override string GetEventName() {
			return "KeyDown";
		}

		protected override void OnEvent(EventArgs eventArgs) {
			if (!(eventArgs is KeyEventArgs))
				return;
			if (Key != ((KeyEventArgs) eventArgs).Key)
				return;
			base.OnEvent(eventArgs);
		}
	}
	
}
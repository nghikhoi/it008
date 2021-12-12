using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors;

namespace UI.Utils.Triggers {
	
	public class ScrollChangeReachTrigger : EventTriggerBase<object> {
		
		public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof(Value), typeof(int), typeof(ScrollChangeReachTrigger)
			, new FrameworkPropertyMetadata(0));
		public static readonly DependencyProperty OperationProperty = DependencyProperty.Register(nameof(Operation), typeof(CompareOperation), typeof(ScrollChangeReachTrigger)
			, new FrameworkPropertyMetadata(CompareOperation.EQUALS));
		public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(nameof(Orientation), typeof(Orientation), typeof(ScrollChangeReachTrigger)
			, new FrameworkPropertyMetadata(Orientation.Vertical));
		public static readonly DependencyProperty DoubleStepModeProperty = DependencyProperty.Register(nameof(DoubleStepMode), typeof(bool), typeof(ScrollChangeReachTrigger)
			, new FrameworkPropertyMetadata(false));

		//Format: X%
		public int Value {
			get { return (int) this.GetValue(ValueProperty); }
			set { this.SetValue(ValueProperty, value); }
		}

		public CompareOperation Operation {
			get { return (CompareOperation) this.GetValue(OperationProperty); }
			set { this.SetValue(OperationProperty, value); }
		}

		public Orientation Orientation {
			get { return (Orientation) this.GetValue(OrientationProperty); }
			set { this.SetValue(OrientationProperty, value); }
		}
		public bool DoubleStepMode {
			get { return (bool) this.GetValue(DoubleStepModeProperty); }
			set { this.SetValue(DoubleStepModeProperty, value); }
		}

		private bool Previous = false;

		protected override string GetEventName() {
			return "ScrollChanged";
		}

		protected override void OnEvent(EventArgs eventArgs) {
			if (!(eventArgs is ScrollChangedEventArgs))
				return;
			if (DoubleStepMode && Previous) {
				Previous = false;
            }
			ScrollChangedEventArgs args = eventArgs as ScrollChangedEventArgs;
			ScrollViewer scroll = (ScrollViewer) args.OriginalSource;
			double current = 0, max = 1;
			switch (Orientation) {
				case Orientation.Horizontal: {
						current = scroll.HorizontalOffset;
						max = scroll.ScrollableWidth;
						break;
					}
				case Orientation.Vertical: {
						current = scroll.VerticalOffset;
						max = scroll.ScrollableHeight;
						break;
                    }
				default: return;
			}
			if (max > 0) {
				int percent = (int) Math.Floor(current / max);
				bool compareResult = CompareOperationUtils.Compare(percent, Value, Operation);
				if (!compareResult)
					return;
				Previous = true;
			}			
			base.OnEvent(eventArgs);
		}
	}
	
}
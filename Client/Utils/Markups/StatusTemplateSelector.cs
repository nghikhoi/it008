using System.Windows;
using System.Windows.Controls;
using UI.Models;
using UI.Models.Message;
using UI.ViewModels;

namespace UI.Utils.Markups
{
    public class StatusTemplateSelector : DataTemplateSelector
    {
		public override DataTemplate SelectTemplate(object item, DependencyObject container)
		{
			FrameworkElement element = container as FrameworkElement;
			if (element != null && item != null && item is bool)
			{

				switch (item)
				{
					case true:
						return element.FindResource("OnlineStatus") as DataTemplate;
					case false:
						return element.FindResource("OfflineStatus") as DataTemplate;
				}
			}
			return null;
		}
	}
}

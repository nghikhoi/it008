using System.Windows;
using System.Windows.Controls;
using UI.Models;
using UI.Models.Message;
using UI.ViewModels;
using UI.ViewModels.Messages;

namespace UI.Utils.Markups
{
    public class GroupBubbleTemplateSelector: DataTemplateSelector
    {
		public override DataTemplate SelectTemplate(object item, DependencyObject container)
		{
			FrameworkElement element = container as FrameworkElement;
			if (element != null && item != null && item is GroupBubbleViewModel)
			{
				GroupBubbleViewModel gr = item as GroupBubbleViewModel;
				bool type= gr.isRecieve;

				switch (type)
				{
					case true:
						return element.FindResource("GroupBubbleSender") as DataTemplate;
					case false:
						return element.FindResource("GroupBubbleReciever") as DataTemplate;
				}
			}
			return null;
		}
	}
}

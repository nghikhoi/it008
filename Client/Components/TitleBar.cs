using UI.Command;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace UI.Components
{
    class TitleBar : BaseComponent
    {
        public ICommand CloseWindowCommand { get; set; }
        public ICommand MaximizeWindowCommand { get; set; }
        public ICommand MinimizeWindowCommand { get; set; }
        public ICommand MouseMoveWindowCommand { get; set; }

        public TitleBar()
        {
            CloseWindowCommand = new RelayCommand<UserControl>(
                parameter => parameter != null,
                parameter => Window.GetWindow(parameter)?.Close());

            MaximizeWindowCommand = new RelayCommand<UserControl>(
                parameter => parameter != null,
                parameter =>
                {
                    Window parentWindow = Window.GetWindow(parameter);
                    if (parentWindow != null)
                    {
                        parentWindow.WindowState = parentWindow.WindowState != WindowState.Maximized
                            ? WindowState.Maximized
                            : WindowState.Normal;
                    }
                });
            MinimizeWindowCommand = new RelayCommand<UserControl>(
               parameter => parameter != null,
               parameter =>
               {
                   Window parentWindow = Window.GetWindow(parameter);
                   if (parentWindow != null)
                   {
                       parentWindow.WindowState = parentWindow.WindowState != WindowState.Minimized
                           ? WindowState.Minimized
                           : WindowState.Normal;
                   }
               });

            MouseMoveWindowCommand = new RelayCommand<UserControl>(
                parameter => parameter != null,
                parameter => Window.GetWindow(parameter)?.DragMove());
        }
    }
}

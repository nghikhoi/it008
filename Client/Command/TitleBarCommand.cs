using UI.Command;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace UI.Components
{
    public class TitleBarDataContext
    {
        public ICommand CloseWindowCommand { get; set; }
        public ICommand MaximizeWindowCommand { get; set; }
        public ICommand MinimizeWindowCommand { get; set; }
        public ICommand MouseMoveWindowCommand { get; set; }

        public TitleBarDataContext() {
            CloseWindowCommand = new RelayCommand<UserControl>(
                parameter => parameter != null,
                parameter => Window.GetWindow(parameter)?.Close());

            MaximizeWindowCommand = new RelayCommand<UserControl>(
                parameter => parameter != null,
                parameter =>
                {
                    Window parentWindow = Window.GetWindow(parameter);
                    if (parentWindow != null) {
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

    public static class TitleBarCommand {
        public static RoutedCommand CloseWindowCommand { get; set; }
        public static RoutedCommand MaximizeWindowCommand { get; set; }
        public static RoutedCommand MinimizeWindowCommand { get; set; }
        public static RoutedCommand MouseMoveWindowCommand { get; set; }

        static TitleBarCommand() {
            CloseWindowCommand = new RoutedCommand();
            CommandManager.RegisterClassCommandBinding(typeof(Window)
                , new CommandBinding(CloseWindowCommand, CloseWindowCommandHandle, TitleBarCommandCanExecute));

            MaximizeWindowCommand = new RoutedCommand();
            CommandManager.RegisterClassCommandBinding(typeof(Window)
                , new CommandBinding(MaximizeWindowCommand, MaximizeWindowCommandHandle, TitleBarCommandCanExecute));

            MinimizeWindowCommand = new RoutedCommand();
            CommandManager.RegisterClassCommandBinding(typeof(Window)
                , new CommandBinding(MinimizeWindowCommand, MinimizeWindowCommandHandle, TitleBarCommandCanExecute));

            MouseMoveWindowCommand = new RoutedCommand();
            CommandManager.RegisterClassCommandBinding(typeof(Window)
                , new CommandBinding(MouseMoveWindowCommand, MouseMoveWindowCommandHandle, TitleBarCommandCanExecute));
        }

        private static void TitleBarCommandCanExecute(object sender, CanExecuteRoutedEventArgs args) {
            object Parameter = args.Parameter;
            if (Parameter == null || !(Parameter is DependencyObject)) {
                args.CanExecute = false;
                return;
            }
            args.CanExecute = true;
        }

        private static void CloseWindowCommandHandle(object sender, ExecutedRoutedEventArgs args) {
            object Parameter = args.Parameter;
            Window.GetWindow(Parameter as DependencyObject)?.Close();
        }

        private static void MaximizeWindowCommandHandle(object sender, ExecutedRoutedEventArgs args) {
            object Parameter = args.Parameter;
            Window parentWindow = Window.GetWindow(Parameter as DependencyObject);
            if (parentWindow != null) {
                parentWindow.WindowState = parentWindow.WindowState != WindowState.Maximized
                    ? WindowState.Maximized
                    : WindowState.Normal;
            }
        }

        private static void MinimizeWindowCommandHandle(object sender, ExecutedRoutedEventArgs args) {
            object Parameter = args.Parameter;
            Window parentWindow = Window.GetWindow(Parameter as DependencyObject);
            if (parentWindow != null) {
                parentWindow.WindowState = parentWindow.WindowState != WindowState.Minimized
                    ? WindowState.Minimized
                    : WindowState.Normal;
            }
        }

        private static void MouseMoveWindowCommandHandle(object sender, ExecutedRoutedEventArgs args) {
            object Parameter = args.Parameter;
            Window.GetWindow(Parameter as DependencyObject)?.DragMove();
        }

    }
}

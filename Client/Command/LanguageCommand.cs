using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using UI.Services;

namespace UI.Command
{
    public static class LanguageCommand {
        public static RoutedCommand ChangeLanguageCommand { get; set; }

        static LanguageCommand() {
            ChangeLanguageCommand = new RoutedCommand();
            CommandManager.RegisterClassCommandBinding(typeof(Window)
                , new CommandBinding(ChangeLanguageCommand, LanguageChangeHandle, LanguageChangeCanExecute));
        }

        private static void LanguageChangeCanExecute(object sender, CanExecuteRoutedEventArgs args) {
            if (args.Command != ChangeLanguageCommand)
                return;
            args.CanExecute = true;
        }

        private static void LanguageChangeHandle(object sender, ExecutedRoutedEventArgs args) {
            if (args.Command != ChangeLanguageCommand)
                return;
            object Parameter = args.Parameter;
            Language.ApplyLanguage(Parameter.ToString());
        }

    }
}

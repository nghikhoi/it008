
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using UI.ViewModels;

namespace UI.Command {
    public static class MediaWindowCommand {

        private static Window MediaGalleryWindow;
        public static RoutedCommand OpenMediaWindowCommand { get; set; }
        public static RoutedCommand CloseMediaWindowCommand { get; set; }

        static MediaWindowCommand() {
            OpenMediaWindowCommand = new RoutedCommand();
            CommandManager.RegisterClassCommandBinding(typeof(Window)
                , new CommandBinding(OpenMediaWindowCommand, OpenMediaWindowCommandHandle, MediaGalleryWindowCanExecute));

            CloseMediaWindowCommand = new RoutedCommand();
            CommandManager.RegisterClassCommandBinding(typeof(Window)
                , new CommandBinding(CloseMediaWindowCommand, CloseMediaWindowCommandHandle, MediaGalleryWindowCanExecute));
        }

        private static void MediaGalleryWindowCanExecute(object sender, CanExecuteRoutedEventArgs args) {
            if (args.Command != OpenMediaWindowCommand && args.Command != CloseMediaWindowCommand)
                return;
            object Parameter = args.Parameter;
            if (Parameter == null || !(Parameter is ConversationViewModel)) {
                args.CanExecute = false;
                return;
            }
            args.CanExecute = true;
        }

        private static void OpenMediaWindowCommandHandle(object sender, ExecutedRoutedEventArgs args) {
            if (args.Command != OpenMediaWindowCommand)
                return;
            object Parameter = args.Parameter;
            if (MediaGalleryWindow == null) {
                MediaGalleryWindow = new MediaGalleryWindow();
                MediaGalleryWindow.Closed += (s, a) => { MediaGalleryWindow = null; };
                MediaGalleryWindow.Show();
            }
            MediaGalleryWindow.DataContext = Parameter;
        }

        private static void CloseMediaWindowCommandHandle(object sender, ExecutedRoutedEventArgs args) {
            if (args.Command != CloseMediaWindowCommand)
                return;
            MediaGalleryWindow?.Close();
        }

    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows;
using System.Windows.Input;
using UI.Components.Download;
using UI.ViewModels;
using UI.Views;

namespace UI.Command {
    public static class DownloadWindowCommand {

        private static Window DownloadWindow;
        public static RoutedCommand OpenDownloadWindowCommand { get; set; }
        public static RoutedCommand CloseDownloadWindowCommand { get; set; }

        static DownloadWindowCommand() {
            OpenDownloadWindowCommand = new RoutedCommand();
            CommandManager.RegisterClassCommandBinding(typeof(Window)
                , new CommandBinding(OpenDownloadWindowCommand, OpenMediaWindowCommandHandle, MediaGalleryWindowCanExecute));

            CloseDownloadWindowCommand = new RoutedCommand();
            CommandManager.RegisterClassCommandBinding(typeof(Window)
                , new CommandBinding(CloseDownloadWindowCommand, CloseMediaWindowCommandHandle, MediaGalleryWindowCanExecute));
        }

        private static void MediaGalleryWindowCanExecute(object sender, CanExecuteRoutedEventArgs args) {
            if (args.Command != OpenDownloadWindowCommand && args.Command != CloseDownloadWindowCommand)
                return;
            args.CanExecute = true;
        }

        private static void OpenMediaWindowCommandHandle(object sender, ExecutedRoutedEventArgs args) {
            if (args.Command != OpenDownloadWindowCommand)
                return;
            OpenWindow();
        }

        private static void CloseMediaWindowCommandHandle(object sender, ExecutedRoutedEventArgs args) {
            if (args.Command != CloseDownloadWindowCommand)
                return;
            DownloadWindow?.Close();
        }

        public static void OpenWindow()
        {
            if (DownloadWindow == null) {
                DownloadWindow = new DownloadWindow();
                DownloadWindow.Closed += (s, a) => { DownloadWindow = null; };
                DownloadWindow.Show();
            }
            DownloadWindow.Focus();
        }

        public static void StartDownload(DownloadItemViewModel item)
        {
            DownloadManagerViewModel.Instance.Add(item);
            OpenWindow();
            item.Start();
        }

    }
}

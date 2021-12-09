using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using MaterialDesignThemes.Wpf;

namespace UI.Command {
    public static class ThemeSelectCommand {

        public static RoutedCommand DarkSelectCommand { get; set; }
        public static RoutedCommand LightSelectCommand { get; set; }

        static ThemeSelectCommand() {
            DarkSelectCommand = new RoutedCommand();
            CommandManager.RegisterClassCommandBinding(typeof(Window)
                , new CommandBinding(DarkSelectCommand, DarkSelectCommandHandle, ThemeSelectCommandCanExecute));

            LightSelectCommand = new RoutedCommand();
            CommandManager.RegisterClassCommandBinding(typeof(Window)
                , new CommandBinding(LightSelectCommand, LightSelectCommandHandle, ThemeSelectCommandCanExecute));
        }

        private static readonly Uri DarkThemeUri =
            new Uri(
                $"pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.{"Dark"}.xaml",
                UriKind.RelativeOrAbsolute);
        private static readonly Uri LightThemeUri =
            new Uri(
                $"pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.{"Light"}.xaml",
                UriKind.RelativeOrAbsolute);

        private static void ThemeSelectCommandCanExecute(object sender, CanExecuteRoutedEventArgs args) {
            if (args.Command != DarkSelectCommand && args.Command != LightSelectCommand)
                return;
            args.CanExecute = true;
        }

        private static void DarkSelectCommandHandle(object sender, ExecutedRoutedEventArgs args) {
            if (args.Command != DarkSelectCommand && args.Command != LightSelectCommand)
                return;
            SetTheme(true);
        }

        private static void LightSelectCommandHandle(object sender, ExecutedRoutedEventArgs args) {
            if (args.Command != DarkSelectCommand && args.Command != LightSelectCommand)
                return;
            SetTheme(false);
        }

        private static void ChangeTheme(Uri uri) {
            Application.Current.Resources.MergedDictionaries[0] = new ResourceDictionary { Source = uri };
        }
        
        private static readonly PaletteHelper _paletteHelper = new PaletteHelper();

        private static void SetTheme(bool isDark) {
            ITheme theme = _paletteHelper.GetTheme();
            IBaseTheme baseTheme = isDark ? new MaterialDesignDarkTheme() : (IBaseTheme) new MaterialDesignLightTheme();
            theme.SetBaseTheme(baseTheme);
            _paletteHelper.SetTheme(theme);
            ResourceDictionary _resourceDictionary = App.Current.Resources;
            if (isDark) {
                _resourceDictionary["BackgroundResource"] = new SolidColorBrush(System.Windows.Media.Color.FromRgb(23,31,33));
                _resourceDictionary["ForegroundResource"] = new SolidColorBrush(Colors.LightGray);
                _resourceDictionary["InputColorResource"] = new SolidColorBrush(Colors.Transparent);
                _resourceDictionary["PrimaryColor"] = new SolidColorBrush(Colors.WhiteSmoke);
                _resourceDictionary["BorderColorResource"] = new SolidColorBrush(Colors.LightGray);
                //App.Current.Resources["BackgroundOpacityBrush"] = new SolidColorBrush(System.Windows.Media.Color.FromArgb(150 ,255, 255, 255));
                //PrimaryIconImage.Source = ImageSourceFromBitmap(Properties.Resources.ca);
                //PrimaryIconImage.Width = 200;
                //PrimaryIconImage.Height = 200;
            } else {
                _resourceDictionary["BackgroundResource"] = new SolidColorBrush(Colors.WhiteSmoke);
                _resourceDictionary["ForegroundResource"] = new SolidColorBrush(Colors.Black);
                _resourceDictionary["InputColorResource"] = new SolidColorBrush(Colors.LightGray);
                _resourceDictionary["BorderColorResource"] = new SolidColorBrush(Colors.Teal);
                _resourceDictionary["PrimaryColor"] = new SolidColorBrush(Colors.Teal);
                //App.Current.Resources["BackgroundOpacityBrush"] = new SolidColorBrush(Colors.LightGray);
                //this.Foreground = Brushes.Black;
            }
        }

    }

}

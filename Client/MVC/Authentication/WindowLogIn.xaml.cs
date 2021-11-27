using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using UI.Models;
using UI.MVC;
using UI.Properties;

namespace UI
{
    /// <summary>
    /// Interaction logic for WindowLogIn.xaml
    /// </summary>
    public partial class WindowLogIn : Window
    {

        public WindowLogIn()
        {
            InitializeComponent();
            VietnameseButton.IsChecked = true;
            //Properties.Settings.Default.ColorMode = "Light";
            //test.IsChecked = Properties.Settings.Default.Theme == BaseTheme.Dark;
            //SetTheme();
        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e) {
            AuthenticationController controller = ModuleContainer.GetModule<Authentication>().controller;
            if (App.IS_LOCAL_DEBUG && !App.SESSION_HOLDER) {
                controller.EnterMainWindow();
                return;
            }
            controller.doLogin(new LoginInfo(UsernameBox.Text, PasswordBox.Password));
        }

        //bool _shown;

        //protected override void OnContentRendered(EventArgs e)
        //{
        //    base.OnContentRendered(e);

        //    if (_shown)
        //        return;

        //    _shown = true;

        //    DoubleAnimation animFadeIn = new DoubleAnimation();
        //    animFadeIn.From = 0;
        //    animFadeIn.To = 1;
        //    animFadeIn.Duration = new Duration(TimeSpan.FromSeconds(1.5));
        //    FillBehavior fill = FillBehavior.HoldEnd;
        //    BeginAnimation(Window.OpacityProperty, animFadeIn, (HandoffBehavior)fill);
        //    //BeginAnimation(Window.OpacityProperty, animFadeIn);

        //    _shown = false;
        //}

        private void SignBtn_Click(object sender, RoutedEventArgs e)
        {
            AuthenticationController controller = ModuleContainer.GetModule<Authentication>().controller;
            //OnContentRendered(e);
            controller.showRegister(); 
        }

        private void ucTitleBar_Loaded_1(object sender, RoutedEventArgs e)
        {

        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void VietnameseButton_Click(object sender, RoutedEventArgs e)
        {
            Lang.Language.ApplyLanguage("vi-VN");
        }

        private void EnglishButton_Click(object sender, RoutedEventArgs e)
        {
            Lang.Language.ApplyLanguage("en-US");
        }
        private void VietnameseButton_Checked(object sender, RoutedEventArgs e)
        {
            if (EnglishButton.IsChecked == true)
                EnglishButton.IsChecked = false;
        }
        private void VietnameseButton_UnChecked(object sender, RoutedEventArgs e)
        {
            if(EnglishButton.IsChecked == false)
            VietnameseButton.IsChecked = true;
        }

        private void EnglishButton_Checked(object sender, RoutedEventArgs e)
        {
            if (VietnameseButton.IsChecked == true)
                VietnameseButton.IsChecked = false;
        }
        private void EnglishButton_UnChecked(object sender, RoutedEventArgs e)
        {
            if(VietnameseButton.IsChecked == false)
            EnglishButton.IsChecked = true;
        }

        private void ThemePanel_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private readonly PaletteHelper _paletteHelper = new PaletteHelper();
        private void SetTheme(bool isDark)
        {
            ITheme theme = _paletteHelper.GetTheme();
            IBaseTheme baseTheme = isDark ? new MaterialDesignDarkTheme() : (IBaseTheme)new MaterialDesignLightTheme();
            theme.SetBaseTheme(baseTheme);
            _paletteHelper.SetTheme(theme);
            if (isDark)
            {
                App.Current.Resources["BackgroundResource"] = new SolidColorBrush(System.Windows.Media.Color.FromRgb(23,31,33));
                App.Current.Resources["ForegroundResource"] = new SolidColorBrush(Colors.LightGray);
                App.Current.Resources["InputColorResource"] = new SolidColorBrush(Colors.Transparent);
                App.Current.Resources["PrimaryColor"] = new SolidColorBrush(Colors.WhiteSmoke);
                App.Current.Resources["BorderColorResource"] = new SolidColorBrush(Colors.LightGray);
                //PrimaryIconImage.Source = ImageSourceFromBitmap(Properties.Resources.ca);
                //PrimaryIconImage.Width = 200;
                //PrimaryIconImage.Height = 200;
            }
            else
            {
                App.Current.Resources["BackgroundResource"] = new SolidColorBrush(Colors.WhiteSmoke);
                App.Current.Resources["ForegroundResource"] = new SolidColorBrush(Colors.Black);
                App.Current.Resources["InputColorResource"] = new SolidColorBrush(Colors.LightGray);
                App.Current.Resources["BorderColorResource"] = new SolidColorBrush(Colors.Teal);
                App.Current.Resources["PrimaryColor"] = new SolidColorBrush(Colors.Teal);
                //this.Foreground = Brushes.Black;
            }
        }

        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteObject([In] IntPtr hObject);

        public ImageSource ImageSourceFromBitmap(Bitmap bmp)
        {
            var handle = bmp.GetHbitmap();
            try
            {
                return Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
            finally { DeleteObject(handle); }
        }

        private void test_Checked(object sender, RoutedEventArgs e) => SetTheme(true);

        private void test_Unchecked(object sender, RoutedEventArgs e) => SetTheme(false);

        //private void SetTheme(bool isDark)
        //{
        //    if (isDark)
        //    {
        //        //test.Content = "Dark Theme";
        //        Settings.Default.Theme = MaterialDesignThemes.Wpf.BaseTheme.Dark;
        //        var converter = new System.Windows.Media.BrushConverter();
        //        BorderLoginWindow.Background = (Brush)converter.ConvertFromString("#121212");
        //        this.Foreground = Brushes.White;
        //    }
        //    else
        //    {
        //        //test.Content = "Light Theme";
        //        Settings.Default.Theme = MaterialDesignThemes.Wpf.BaseTheme.Light;
        //        BorderLoginWindow.Background = new SolidColorBrush(Theme.Light.MaterialDesignBackground); // sua mau lai (theme light)
        //        this.Foreground = Brushes.Black;
        //    }

        //    Settings.Default.Save();
        //    ((App)Application.Current).SetTheme(Settings.Default.Theme);
        //}




        //private void test_Checked(object sender, RoutedEventArgs e)
        //{
        //    Properties.Settings.Default.ColorMode = "Black";
        //    Properties.Settings.Default.Save();
        //}

        //private void test_Unchecked(object sender, RoutedEventArgs e)
        //{
        //    Properties.Settings.Default.ColorMode = "Light";
        //    Properties.Settings.Default.Save();
        //}

    }



    //public class NotEmptyValidationRule : ValidationRule
    //{
    //    public string Message { get; set; }

    //    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    //    {
    //        if (string.IsNullOrWhiteSpace(value?.ToString()))
    //        {
    //            return new ValidationResult(false, Message ?? "A value is required");
    //        }
    //        return ValidationResult.ValidResult;
    //    }
    //}
}

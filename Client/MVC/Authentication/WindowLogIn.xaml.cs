using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using UI.Models;
using UI.MVC;

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

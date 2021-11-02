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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Show();
            this.Close();
        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            HomeWindow home = new HomeWindow();
            home.Show();
            this.Close();
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

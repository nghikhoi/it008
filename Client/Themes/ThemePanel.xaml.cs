using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UI.Themes
{
    /// <summary>
    /// Interaction logic for ThemePanel.xaml
    /// </summary>
    public partial class ThemePanel : UserControl
    {
        public ThemePanel()
        {
            InitializeComponent();
            
        }

        private static void SetPrimaryColor(Color color)
        {
            //This is the API in 4.2.1
            PaletteHelper paletteHelper = new PaletteHelper();
            //For versions after this point use MaterialDesignThemes.Wpf.Theming.ThemeManager

            //Get current theme
            var theme = paletteHelper.GetTheme();

            //Apply primary color
            theme.SetPrimaryColor(color);

            //Apply theme to application
            paletteHelper.SetTheme(theme);

        }

        //private void SetPrimaryColor(Color color)
        //{
        //    string s = "";
        //    try
        //    {
        //        if (color == Colors.Red)
        //        {
        //            s = "Red";
        //        } 
        //        if (color == Colors.BlueViolet)
        //        {
        //            s = "deeppurple";
        //        }
        //        if (color == Colors.Teal)
        //        {
        //            s = "teal";
        //        }
        //        if (color == Colors.Gray)
        //        {
        //            s = "grey";
        //        }
        //        if (color == Colors.OrangeRed)
        //        {
        //            s = "deeporange";
        //        }
        //        if (color == Colors.Yellow)
        //        {
        //            s = "yellow";
        //        }
        //        if (color == Colors.DeepSkyBlue)
        //        {
        //            s = "blue";
        //        }
        //        if (color == Colors.Black)
        //        {
        //            s = "black";
        //        }
        //        if (color == Colors.Lime)
        //        {
        //            s = "lime";
        //        }
        //        ChangeTheme(new Uri($"pack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Primary/MaterialDesignColor.{s}.xaml", UriKind.RelativeOrAbsolute));
        //        //switch (color)
        //        //{
        //        //    case Color.Red:
        //        //        ChangeTheme(new Uri($"pack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Primary/MaterialDesignColor.{"Red"}.xaml", UriKind.RelativeOrAbsolute));
        //        //        break;
        //        //    default:
        //        //        ChangeTheme(new Uri($"pack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Primary/MaterialDesignColor.{"Red"}.xaml", UriKind.RelativeOrAbsolute));
        //        //        break;
        //        //}
        //    }
        //    catch { }
        //}

        //void ChangeTheme(Uri uri)
        //{
        //    System.Windows.Application.Current.Resources.MergedDictionaries.RemoveAt(2);
        //    System.Windows.Application.Current.Resources.MergedDictionaries.Insert(2, new ResourceDictionary() { Source = uri });
        //}

        private void tealTheme_Checked(object sender, RoutedEventArgs e)
        {
            SetPrimaryColor(Colors.Teal);
        }

        private void purpleTheme_Checked(object sender, RoutedEventArgs e)
        {
            SetPrimaryColor(Colors.BlueViolet);
        }

        private void grayTheme_Checked(object sender, RoutedEventArgs e)
        {
            SetPrimaryColor(Colors.Gray);
        }

        private void limeTheme_Checked(object sender, RoutedEventArgs e)
        {
            SetPrimaryColor(Colors.Lime);
        }

        private void orangeRedTheme_Checked(object sender, RoutedEventArgs e)
        {
            SetPrimaryColor(Colors.OrangeRed);
        }

        private void blackTheme_Checked(object sender, RoutedEventArgs e)
        {
            SetPrimaryColor(Colors.Black);
        }

        private void RedTheme_Checked(object sender, RoutedEventArgs e)
        {
            SetPrimaryColor(Colors.Red);
        }

        private void DeepSkyBlueTheme_Checked(object sender, RoutedEventArgs e)
        {
            SetPrimaryColor(Colors.DeepSkyBlue);
        }

        private void yellowTheme_Checked(object sender, RoutedEventArgs e)
        {
            SetPrimaryColor(Colors.Yellow);
        }


        ////Dark/Light
        //private void darkLightTheme_Checked(object sender, RoutedEventArgs e) => SetTheme();

        //private void darkLightTheme_Unchecked(object sender, RoutedEventArgs e) => SetTheme();

        //private void SetTheme()
        //{
        //    if (darkLightTheme.IsChecked == true)
        //    {
        //        //Text.Text = "Dark Theme";
        //        Properties.Settings.Default.Theme = MaterialDesignThemes.Wpf.BaseTheme.Dark;
        //    }
        //    else
        //    {
        //        //Text.Text = "Light Theme";
        //        Properties.Settings.Default.Theme = MaterialDesignThemes.Wpf.BaseTheme.Light;
        //    }

        //    Properties.Settings.Default.Save();
        //    ((App)Application.Current).SetTheme(Properties.Settings.Default.Theme);
        //}
    }
}

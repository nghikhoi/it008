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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using UI.Models;
using UI.MVC;

namespace UI
{
    /// <summary>
    /// Interaction logic for WindowRegister.xaml
    /// </summary>
    public partial class WindowRegister : Window
    {
        public WindowRegister()
        {
            InitializeComponent();
        }

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            AuthenticationController controller = ModuleContainer.GetModule<Authentication>().controller;
            //OnContentRendered(e);
            controller.showLogin();
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
        //    animFadeIn.Duration = new Duration(TimeSpan.FromSeconds(1));
        //    FillBehavior fill = FillBehavior.HoldEnd;
        //    BeginAnimation(Window.OpacityProperty, animFadeIn, (HandoffBehavior)fill);
        //    //BeginAnimation(Window.OpacityProperty, animFadeIn);

        //    _shown = false;
        //}

        private void signUpBth_Click(object sender, RoutedEventArgs e) {
            RegisterInfo info = new RegisterInfo(FirstNameBox.Text, LastNameBox.Text, UsernameBox.Text,
                PasswordBox.Password, BirthdayPicker.SelectedDate.Value, Gender.Male); //TODO update gender
            
            AuthenticationController controller = ModuleContainer.GetModule<Authentication>().controller;
            controller.doRegister(info);
        }
        
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
    }
}

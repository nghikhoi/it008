using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//todo: nut dang ky cho sign up
namespace UI.Command
{
    public class SignUpClickCommand : Command.BaseCommand
    {
        private void signUpBth_Click(object sender, RoutedEventArgs e)
        {
            RegisterInfo info = new RegisterInfo(FirstNameBox.Text, LastNameBox.Text, UsernameBox.Text,
                PasswordBox.Password, BirthdayPicker.SelectedDate.Value, Gender.Male); //TODO update gender

            AuthenticationController controller = ModuleContainer.GetModule<Authentication>().controller;
            controller.doRegister(info);
        }
    }
}
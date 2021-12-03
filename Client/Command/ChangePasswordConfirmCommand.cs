using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Command
{
    class ChangePasswordConfirmCommand:BaseCommand
    {
        private void ConfirmButton_OnClick(object sender, RoutedEventArgs e)
        {
            string oldPassword = this.PasswordBox.Password;
            string newPassword = this.ChangePasswordBox.Password;
            string newPasswordConfirm = this.ConfirmNewPasswordBox.Password;
            if (String.CompareOrdinal(newPassword, newPasswordConfirm) != 0)
            {
                //TODO show error
                return;
            }

            ModifyPassword packet = new ModifyPassword();
            packet.OldPassword = oldPassword;
            packet.NewPassword = newPassword;
            //TODO send packet
        }
    }
}

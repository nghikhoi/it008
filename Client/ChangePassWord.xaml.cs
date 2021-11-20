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
using System.Windows.Shapes;
using UI.Network.Packets.AfterLoginRequest.Profile;

namespace UI
{
    /// <summary>
    /// Interaction logic for ChangePassWord.xaml
    /// </summary>
    public partial class ChangePassWord : Window
    {
        public ChangePassWord()
        {
            InitializeComponent();
        }
        
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ConfirmButton_OnClick(object sender, RoutedEventArgs e) {
            string oldPassword = this.PasswordBox.Password;
            string newPassword = this.ChangePasswordBox.Password;
            string newPasswordConfirm = this.ConfirmNewPasswordBox.Password;
            if (String.CompareOrdinal(newPassword, newPasswordConfirm) != 0) {
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

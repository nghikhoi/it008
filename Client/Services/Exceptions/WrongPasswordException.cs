using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace UI.Services.Exceptions
{
    public class WrongPasswordException : Exception
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }

        public WrongPasswordException(string oldPassword, string newPassword)
        {
            OldPassword = oldPassword;
            NewPassword = newPassword;
        }

        public WrongPasswordException(string message, string oldPassword, string newPassword) : base(message)
        {
            OldPassword = oldPassword;
            NewPassword = newPassword;
        }

        public WrongPasswordException(string message, Exception innerException, string oldPassword, string newPassword) : base(message, innerException)
        {
            OldPassword = oldPassword;
            NewPassword = newPassword;
        }
    }
}

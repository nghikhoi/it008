using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace UI.Services.Exceptions
{
    public class AlreadyExistsUserException : Exception
    {
        public string Username { get; set; }

        public AlreadyExistsUserException(string username)
        {
            Username = username;
        }

        public AlreadyExistsUserException(string message, string username) : base(message)
        {
            Username = username;
        }

        public AlreadyExistsUserException(string message, Exception innerException, string username) : base(message, innerException)
        {
            Username = username;
        }
    }
}

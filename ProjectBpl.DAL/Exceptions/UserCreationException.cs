using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using ProjectBpl.DAL.Entities;

namespace ProjectBpl.DAL.Exceptions
{
    internal enum UserCreationExceptionEnum
    {
        UserNameExists, EmailExists, PasswordToWeak
    }
    [Serializable]
    internal class UserCreationException : Exception
    {
        private readonly string _message;
        public UserCreationException(params UserCreationExceptionEnum[] reasons)
        {
            string message = "";
            foreach (var reason in reasons)
            {
                message += "\n ";
                switch (reason)
                {
                    case UserCreationExceptionEnum.EmailExists:
                        message += "An account already exists with this email";
                        break;
                    case UserCreationExceptionEnum.UserNameExists:
                        message += "An account already exists with this username";
                        break;
                    case UserCreationExceptionEnum.PasswordToWeak:
                        message += "The password is to weak.";
                        break;
                }
            }

            _message = message;

        }

        public override string Message => _message;
    }
}

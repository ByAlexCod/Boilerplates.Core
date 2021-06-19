using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectBpl.DAL.Inerfaces
{
    public interface IUserRegister
    {
        string Username { get; set; }
        string Password { get; set; }
        string  Email { get; set; }
    }
}

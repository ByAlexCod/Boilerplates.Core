using ProjectBpl.DAL.Inerfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectBpl.API.Dtos
{
    public class UserDTO : IUser, IUserGet
    {
        public int UserId { get; set; }

        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}

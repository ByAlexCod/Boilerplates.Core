using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using ProjectBpl.DAL.Inerfaces;

namespace ProjectBpl.DAL.Entities
{
    public class User : IUser, IUserGet, IUserRegister
    {
        public int UserId { get; set; }
        [StringLength(255)]
        
        public string Username { get; set; }
        [StringLength(255)]
        public string Email { get; set; }
        [StringLength(50)]
        public string Provider { get; set; }
        [StringLength(500)]

        public string Password { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectBpl.DAL.Inerfaces
{
    public interface IUser: IUserRegister
    {
        public int UserId { get; set; }
    }
}

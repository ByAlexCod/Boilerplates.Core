using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ProjectBpl.DAL.CoreServices;
using ProjectBpl.DAL.Entities;
using ProjectBpl.DAL.Inerfaces;

namespace ProjectBpl.Tests
{
    public class TestUtils
    {
        public static string DEFAULT_EMAIL = "alex@example.org";
        public static string DEFAULT_USERNAME = "nestor1807";
        public static string DEFAULT_PASSWORD = "MySuperPassword1234*";


        public static async Task<IUserGet> CreateBaseUser(UserService service)
        {
            var user = await service.Create(new User()
                { Email = DEFAULT_EMAIL, Password = DEFAULT_PASSWORD, Username = DEFAULT_USERNAME });
            return user;
        } 
    }
}

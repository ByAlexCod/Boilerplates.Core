using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProjectBpl.DAL.Entities;
using ProjectBpl.DAL.Exceptions;
using ProjectBpl.DAL.Inerfaces;

namespace ProjectBpl.DAL.CoreServices
{
    public class UserService
    {

        private bool CheckPasswordStrength(string password)
        {
            return password.Length >= 8 && password.Any(char.IsDigit) && password.Any(char.IsLetter);
        }

        public async Task<IUserGet> Create(IUserRegister userModel, string provider = "internal")
        {
            var errors = new List<UserCreationExceptionEnum>();
            if(await GetUserByEmail(userModel.Email) != null) errors.Add(UserCreationExceptionEnum.EmailExists);
            if (await GetUserByUsername(userModel.Email) != null) errors.Add(UserCreationExceptionEnum.UserNameExists);
            if((provider == "internal" || string.IsNullOrEmpty(provider)) && !CheckPasswordStrength(userModel.Password)) errors.Add(UserCreationExceptionEnum.PasswordToWeak);
            if(errors.Count > 0) throw new UserCreationException(errors.ToArray());

            await using var ctx = new EFCoreContext();
            var entity = ctx.Users.Add(new User()
            {
                Email = userModel.Email, Password = BCrypt.Net.BCrypt.HashPassword(userModel.Password),
                Username = userModel.Username
            });
            await ctx.SaveChangesAsync();
            return entity.Entity;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IUserGet> GetUserById(int id)
        {
            using (var ctx = new EFCoreContext())
            {
                return (await ctx.Users.Where(u => u.UserId == id).FirstAsync());
            }
        }

        public async Task<IUserGet> GetUserByEmail(string email)
        {
            using (var ctx = new EFCoreContext())
            {
                return (await ctx.Users.Where(u => u.Email == email).FirstOrDefaultAsync());
            }
        }

        public async Task<IUserGet> GetUserByUsername(string username)
        {
            using (var ctx = new EFCoreContext())
            {
                return (await ctx.Users.Where(u => u.Username == username).FirstOrDefaultAsync());
            }
        }

        public async Task<bool> CheckPassword(string emailOrUsername, string password)
        {
            using (var ctx = new EFCoreContext())
            {
                var user = await ctx.Users.Where(u => u.Email == emailOrUsername || u.Username == emailOrUsername).FirstOrDefaultAsync();
                if (user == null) return false;
                return BCrypt.Net.BCrypt.Verify(password, user.Password);
            }
        }

        private async Task<User> GetDBUserById(int id)
        {
            using (var ctx = new EFCoreContext())
            {
                return (await ctx.Users.Where(u => u.UserId == id).FirstAsync());
            }
        }

        private async Task<User> GetDBUserByEmail(string email)
        {
            using (var ctx = new EFCoreContext())
            {
                return (await ctx.Users.Where(u => u.Email == email).FirstAsync());
            }
        }

        public async Task<bool> DeleteUser(int userId)
        {
            using (var ctx = new EFCoreContext())
            {
                ctx.Users.Remove(await GetDBUserById(userId));
                await ctx.SaveChangesAsync();

                return true;
            }
        }

        public async Task<bool> DeleteUserByEmail(string email)
        {
            using (var ctx = new EFCoreContext())
            {
                ctx.Users.Remove(await GetDBUserByEmail(email));
                await ctx.SaveChangesAsync();
                return true;
            }
        }

        public async Task UpdatePassword(int userId, string newPassword)
        {
            using (var ctx = new EFCoreContext())
            {
                var user = await ctx.Users.FirstOrDefaultAsync(a => a.UserId == userId);
                user.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
                await ctx.SaveChangesAsync();
                
            }
        }

    }
}

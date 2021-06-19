using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using ProjectBpl.API.Dtos;
using ProjectBpl.DAL.CoreServices;

namespace ProjectBpl.API.Services
{
    public class AuthService
    {
        private readonly string _jwtIssuer;
        private readonly string _jwtKey;
        private readonly UserService _userService;

        public AuthService(string jwtIssuer, string jwtKey, UserService userService)
        {
            _jwtIssuer = jwtIssuer;
            _jwtKey = jwtKey;
            _userService = userService;
        }
        internal string GenerateJSONWebToken(LoginModel loginModel)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_jwtIssuer,
                _jwtIssuer,
                null,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        internal async Task<LoginModel> AuthenticateUser(LoginModel login)
        {
            LoginModel user = null;
   
            if (await _userService.CheckPassword(login.UserName, login.Password))
            {
                user = login;
            }
            return user;
        }


    }
}

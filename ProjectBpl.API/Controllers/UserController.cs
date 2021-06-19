using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProjectBpl.API.Dtos;
using ProjectBpl.DAL.CoreServices;
using ProjectBpl.DAL.Inerfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ProjectBpl.API.Services;
using ProjectBpl.DAL.Entities;

namespace ProjectBpl.API.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class UserController : ControllerBase
    {
        

        private readonly ILogger<UserController> _logger;
        private readonly UserService _userService;
        private readonly AuthService _authService;

        public UserController(ILogger<UserController> logger, UserService userService, AuthService authService)
        {
            _logger = logger;
            _userService = userService;
            _authService = authService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IUserGet> Register([FromBody]UserDTO model)
        {
            return await _userService.Create(model);
        }


        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel data)
        {
            IActionResult response = Unauthorized();
            var user = await _authService.AuthenticateUser(data);
            if (user != null)
            {
                var tokenString = _authService.GenerateJSONWebToken(user);
                response = Ok(new { Token = tokenString, Message = "Success" });
            }
            return response;
        }

        [HttpPost("auth/google")]
        [ProducesDefaultResponseType]
        public async Task<JsonResult> GoogleLogin([FromBody] string idToken)
        {
            GoogleJsonWebSignature.Payload payload;
            try
            {
                payload = await GoogleJsonWebSignature.ValidateAsync(idToken, new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = new[] { "YOUR_CLIENT_ID" }
                });

                var user = await GetOrCreateExternalLoginUser("google", payload.Subject, payload.Email, payload.GivenName + "." + payload.FamilyName);
                var token = _authService.GenerateJSONWebToken(new LoginModel()
                {
                    UserName = user.Username,
                    Password = ""
                });
                return new JsonResult(token);
            }
            catch
            {
                return new JsonResult(Unauthorized());
            }

        }



        public async Task<IUserGet> GetOrCreateExternalLoginUser(string provider, string key, string email, string username)
        {
            // Login already linked to a user
            var user = await _userService.GetUserByEmail(email);
            if (user != null)
                return user;

            
                user = await _userService.Create(new User()
                {
                    Email = email,
                    Password = Guid.NewGuid().ToString(),
                    Username = username

                }, provider = "google");
                // No user exists with this email address, we create a new one
                

            

           
            return user;

        }

    }
}

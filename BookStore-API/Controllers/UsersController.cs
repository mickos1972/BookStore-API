using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BookStore_API.Data.DTOs;
using BookStore_API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookStore_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILoggerService _logger;
        private readonly IConfiguration _config;

        public UsersController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, ILoggerService logger,
            IConfiguration configuration)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            _config = configuration;
        }

        [Route("register")]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] UserDTO userDTO)
        {
            try
            {
                var userName = userDTO.EmailAddress;
                var password = userDTO.Password;

                var user = new IdentityUser { Email = userName, UserName = userName };

                var result = await _userManager.CreateAsync(user, password);

                if(!result.Succeeded)
                    return InternalError("Somethings gone wrong in register");

                return Ok(new { result.Succeeded });

            }
            catch (Exception ex)
            {
                return InternalError("Somethings gone wrong in register" + ex.Message);
            }
        }

        /// <summary>
        /// User Login
        /// </summary>
        /// <param name="userDTO"></param>
        /// <returns></returns>
        [Route("login")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] UserDTO userDto)
        {
            //var result = await _signInManager.PasswordSignInAsync(userDto.UserName, userDto.Password, false, false);

            //var test = await _userManager.FindByNameAsync(userDto.UserName);

            try
            {
                //if (result.Succeeded)
                //{
                //var user = await _userManager.FindByNameAsync(userDto.UserName);

                //IdentityUser user = await _userManager.FindByNameAsync("customerOne");
                //IdentityUser user = await _userManager.FindByEmailAsync("customer@gmail.com");
                //this is a fake user
                var user = new IdentityUser
                    {
                        UserName = "customerOne",
                        Email = "customer@gmail.com"
                    };

                var tokenString = await GenerateJSONWebToken(user);

                return Ok(new { token = tokenString });

                //}
                //return Unauthorized(userDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error logging in {ex.Message}");
                return InternalError($"{ex.Message} - {ex.InnerException}");
            }
        }

        private ObjectResult InternalError(string message)
        {
            _logger.LogError(message);
            return StatusCode(500, $"Somethings not right .. {message}");
        }

        private async Task<string> GenerateJSONWebToken(IdentityUser user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim("UserName", user.UserName.ToString())               
            };

            var roles = await _userManager.GetRolesAsync(user);

            claims.AddRange(roles.Select(r => new Claim(ClaimsIdentity.DefaultRoleClaimType, r)));

            var token = new JwtSecurityToken(_config["Jwt: Issuer"],
                _config["Jwt: Issuer"],
                claims,
                null,
                expires: DateTime.Now.AddMinutes(5),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

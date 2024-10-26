using Microsoft.AspNetCore.Mvc;
using PetApp.Utility.DTOs;
using PetApp.Utility;
using PetApp.Services;
using PetApp.Services.Repository;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using PetApp.Services.Validators;
using PetApp.Services.Services;
using Amazon.Runtime.Internal;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace PetApp.API.Controllers
{
    [Tags("Users")]
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private UserService _userService;
        private UserValidator _userValidator;

        public UserController(UserValidator userValidator, UserService userService)
        {
            _userService=userService;
            _userValidator=userValidator;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDB>> Register(
            [FromHeader, BindRequired] string Username, [FromHeader, BindRequired] string Password,
            [FromHeader] string Email, [FromHeader] string Name,
            [FromHeader] long Phone)
        {
            UserRequest request = new()
            {
                Username = Username,
                Password = Password,
                Email = Email,
                Name = Name,
                Phone = Phone
            };
            var inValid = _userValidator.ValidateNew(request);
            if (inValid != null)
            {
                return BadRequest(inValid);
            }
        
            var user = await _userService.CreateUser(request);
            UserDB instance = user as UserDB;
            if (instance is null)
            {
                return BadRequest(user);
            }
            return Ok(user);
        }

        [HttpGet("login")]
        public async Task<ActionResult<LogedUser>> Login(
            [FromHeader, BindRequired] string Username, [FromHeader, BindRequired] string Password)
        {
            var user = await _userService.LoginUser(Username, Password);
            var inValid = _userValidator.ValidateExistingUser(user);
            if (inValid != null)
            {
                return BadRequest(inValid);
            }
            return Ok(user);
        }

        [HttpGet("UsersList")]
        public async Task<ActionResult> GetAll()
        {
            var users = await _userService.GetAllUsers();
            return Ok(users);
        }
    }
}
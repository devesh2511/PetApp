using Amazon.Runtime.Internal;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using PetApp.Services.Repository;
using PetApp.Utility;
using PetApp.Utility.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetApp.Services.Services
{
    public class UserService
    {
        private UserServiceDB _userServiceDB;
        private JWTService _jwtService;
        public UserService(UserServiceDB userServiceDB, JWTService jwtService)
        {
            _userServiceDB = userServiceDB;
            _jwtService=jwtService;
        }

        public async Task<dynamic> CreateUser(UserRequest request)
        {
            var userExist = await _userServiceDB.GetAsync(request.Username);
            if (userExist != null)
            {
                return ("Username " + request.Username + " already exist!");
            }
            _jwtService.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
            var user = new UserDB
            {
                Username = request.Username,
                Name = request.Name,
                Email = request.Email,
                Phone = request.Phone,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
            };
            await _userServiceDB.CreateAsync(user);
            return user;
        }

        public async Task<LogedUser> LoginUser(string username, string Password)
        {
            var user = await _userServiceDB.GetAsync(username);
            if (user is null)
            {
                return null;
            }
            LogedUser logedUser = new LogedUser
            {
                Username = user.Username,
                Name = user.Name,
                Email = user.Email,
                Phone = user.Phone
            };
            if (!_jwtService.VerifyPasswordHash(Password, user.PasswordHash, user.PasswordSalt))
            {
                return logedUser;
            }
            string token = _jwtService.CreateToken(user);
            logedUser.Token = token;
            return logedUser;
        }

        public async Task<List<User>> GetAllUsers()
        {
            var usersDB = await _userServiceDB.GetAsync();
            var userList = new List<User>();
            foreach (var userDB in usersDB)
            {
                userList.Add(new User
                {
                    Username = userDB.Username,
                    Name = userDB.Name,
                    Email = userDB.Email,
                    Phone = userDB.Phone
                }
                );
            }
            return userList;
        }
    }
}

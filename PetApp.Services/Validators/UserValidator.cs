using Amazon.Runtime.Internal;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using PetApp.Services.Repository;
using PetApp.Services.Services;
using PetApp.Utility;
using PetApp.Utility.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Net.Mail;

namespace PetApp.Services.Validators
{
    public class UserValidator
    {
        private UserServiceDB _userServiceDB;
        private UserService _userService;
        private JWTService _jwtService;
        public UserValidator(UserServiceDB userServiceDB, UserService userService, JWTService jwtService)
        {
            _userServiceDB = userServiceDB;
            _userService = userService;
            _jwtService=jwtService;
        }

        public string ValidateNew(UserRequest request)
        {
            var emailValidation = new EmailAddressAttribute();
            if (!emailValidation.IsValid(request.Email))
            {
                return "Email Address is Invalid";
            }
            else if (request.Phone.ToString().Length != 10)
            {
                return "Phone Number is Invalid";
            }
            return null;
        }

        public string ValidateExistingUser(LogedUser user)
        {
            if (user is null)
            {
                return "Username does not exist!";
            }
            if (user.Token == null)
            {
                return "Incorrect Password!";
            }
            return null;
        }
    }
}

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
using PetApp.Utility.DTOs.Admin;

namespace PetApp.API.Controllers
{
    [Tags("MyPets")]
    [Route("/{user}")]
    [ApiController]
    public class MyPetsController : ControllerBase
    {
        private MyPetService _myPetService;
        private MyPetValidator _myPetValidator;

        public MyPetsController(MyPetValidator myPetValidator, MyPetService myPetService)
        {
            _myPetService = myPetService;
            _myPetValidator = myPetValidator;
        }

        [HttpPost("addPet")]
        public async Task<ActionResult<myPetDB>> Register([FromRoute, BindRequired] string user,
            [FromHeader, BindRequired] string petName, 
            [FromHeader, BindRequired] int petId, 
            [FromHeader, BindRequired] PetSex sex,
            [FromHeader, BindRequired] DateOnly birthdate)
        {
            petRequest request = new()
            {
                owner = user,
                petName = petName,
                birthDate = birthdate,
                sex = sex,
                petId = petId,
            };

            //var inValid = _myPetValidator.ValidateNew(request);
            //if (inValid != null)
            //{
            //    return BadRequest(inValid);
            //}

            var myNewPet = await _myPetService.CreateMyPet(request);
            myPetDB instance = myNewPet as myPetDB;
            if (instance is null)
            {
                return BadRequest(myNewPet);
            }
            return Ok(myNewPet);
        }

        //[HttpGet("getPetDetails")]
        //public async Task<ActionResult<LogedUser>> Login([FromRoute, BindRequired] string user,
        //    [FromHeader, BindRequired] string Username, [FromHeader, BindRequired] string Password)
        //{
        //    var loginUser = await _userService.LoginUser(Username, Password);
        //    var inValid = _userValidator.ValidateExistingUser(loginUser);
        //    if (inValid != null)
        //    {
        //        return BadRequest(inValid);
        //    }
        //    return Ok(user);
        //}
        
        [HttpGet(nameof(GetAllPets))]
        public async Task<ActionResult<IEnumerable<myPetDB>>> GetAllPets([FromRoute, BindRequired] string user)
        {
            var users = await _myPetService.GetAllUserPets(user);
            return Ok(users);
        }

        [HttpGet(nameof(GetOnePet))]
        public async Task<ActionResult<myPetDB>> GetOnePet([FromRoute, BindRequired] string user,
            [FromHeader, BindRequired] string petName,
            [FromHeader, BindRequired] int petId)
        {
            var users = await _myPetService.GetUserPet(user, petName, petId);
            return Ok(users);
        }
    }
}
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
using PetApp.Services.Services.Admin;

namespace PetApp.API.Controllers.Admin
{
    [Tags("Admin - Pets")]
    [Route("/{admin}")]
    [ApiController]
    public class PetsController : ControllerBase
    {
        private PetService _petService;
        // private MyPetValidator _myPetValidator;

        public PetsController(PetService petService)
        {
            _petService = petService;
            // _myPetValidator = myPetValidator;
        }

        [HttpGet(nameof(RegisterNewPet))]
        public async Task<ActionResult<PetsDB>> RegisterNewPet([FromRoute, BindRequired] string admin,
            [FromHeader, BindRequired] PetClass petClass, [FromHeader, BindRequired]  PetType petType, [FromHeader] string petBreed)
        {
            PetsDB request = new()
            {
                admin = admin,
                petClass = petClass.ToString(),
                petType = petType.ToString(),
                petBreed = petBreed ?? null,
                createdOn = DateOnly.FromDateTime(DateTime.Now)
            };

            var NewPet = await _petService.CreatePet(request);
            PetsDB instance = NewPet as PetsDB;
            if (instance is null)
            {
                return BadRequest(NewPet);
            }
            return Ok(NewPet);
        }

        [HttpGet(nameof(GetAllPet))]
        public async Task<ActionResult> GetAllPet()
        {
            var pets = await _petService.GetAllPets();
            return Ok(pets);
        }

        [HttpGet(nameof(GetPetById))]
        public async Task<ActionResult> GetPetById([FromHeader, BindRequired] int petId)
        {
            var pets = await _petService.GetPet(petId);
            return Ok(pets);
        }

        [HttpGet(nameof(GetPetByClass))]
        public async Task<ActionResult> GetPetByClass([FromHeader, BindRequired] PetClass petClass)
        {
            var pets = await _petService.GetByPetClass(petClass.ToString());
            return Ok(pets);
        }

        [HttpGet(nameof(GetPetByType))]
        public async Task<ActionResult> GetPetByType([FromHeader, BindRequired] PetType petType)
        {
            var pets = await _petService.GetByPetType(petType.ToString());
            return Ok(pets);
        }

    }
}
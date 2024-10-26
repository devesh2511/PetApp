using Amazon.Runtime.Internal;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using PetApp.Services.Repository;
using PetApp.Services.Repository.Admin;
using PetApp.Utility;
using PetApp.Utility.DTOs;
using PetApp.Utility.DTOs.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetApp.Services.Services.Admin
{
    public class PetService
    {
        private PetRepository _petRepository;
        private CompareStrings _compareStrings;
        public PetService(PetRepository petRepository, CompareStrings compareStrings)
        {
            _petRepository = petRepository;
            _compareStrings = compareStrings;
        }

        public async Task<dynamic> CreatePet(PetsDB request)
        {
            var allPets = await _petRepository.GetAllPets();

            foreach (var pet in allPets)
            {
                var breedExist = _compareStrings.IsStringEqual(request.petBreed, pet.petBreed);
                if (breedExist.Item1 is true)
                {
                    return ("Pet " + request.petBreed + " already exist!"); ;
                }
                request.petBreed = breedExist.Item2;
            }

            await _petRepository.AddNewPet(request);
            return request;
        }

        //public async Task<LogedUser> LoginUser(string username, string Password)
        //{
        //    var user = await _userServiceDB.GetAsync(username);
        //    if (user is null)
        //    {
        //        return null;
        //    }
        //    LogedUser logedUser = new LogedUser
        //    {
        //        Username = user.Username,
        //        Name = user.Name,
        //        Email = user.Email,
        //        Phone = user.Phone
        //    };
        //    if (!_jwtService.VerifyPasswordHash(Password, user.PasswordHash, user.PasswordSalt))
        //    {
        //        return logedUser;
        //    }
        //    string token = _jwtService.CreateToken(user);
        //    logedUser.Token = token;
        //    return logedUser;
        //}

        public async Task<IEnumerable<PetsDB>> GetAllPets()
        {
            var allPets = await _petRepository.GetAllPets();
            return allPets;
        }

        public async Task<PetsDB> GetPet(int petId)
        {
            var pet = await _petRepository.GetOnePet(petId);
            return pet;
        }

        public async Task<IEnumerable<PetsDB>> GetByPetClass(string petClass)
        {
            var allPets = await _petRepository.GetAllByPetClass(petClass);
            return allPets;
        }

        public async Task<IEnumerable<PetsDB>> GetByPetType(string petType)
        {
            var allPets = await _petRepository.GetAllByPetType(petType);
            return allPets;
        }
    }
}

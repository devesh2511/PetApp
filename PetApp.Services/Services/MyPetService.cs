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

namespace PetApp.Services.Services
{
    public class MyPetService
    {
        private MyPetRepository _myPetRepository;
        private PetRepository _petRepository;
        public MyPetService(MyPetRepository myPetRepository, PetRepository petRepository)
        {
            _myPetRepository = myPetRepository;
            _petRepository=petRepository;
        }

        public async Task<dynamic> CreateMyPet(petRequest request)
        {
            var userPets = await _myPetRepository.GetUserPets(request.owner);

            var petExists = userPets.Find(x=>x.birthDate == request.birthDate && 
            x.petId == request.petId && x.petName == request.petName);

            if (petExists != null)
            {
                return ("Pet " + request.petName + " already exist!"); ;
            }

            var newPet = new myPetDB
            {
                owner = request.owner,
                petName = request.petName,
                birthDate = request.birthDate,
                sex = request.sex.ToString(),
                petId = request.petId
            };

            await _myPetRepository.CreateAsync(newPet);
            return newPet;
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

        public async Task<IEnumerable<myPetResponse>> GetAllUserPets(string owner)
        {
            var myPets = await _myPetRepository.GetUserPets(owner);

            var petsList = new List<myPetResponse>();
            foreach (var item in myPets)
            {
                DateOnly now = DateOnly.FromDateTime(DateTime.Now);
                int age = (now.DayNumber - item.birthDate.DayNumber)/365;
                var petDetail = await _petRepository.GetOnePet(item.petId);
                petsList.Add(new myPetResponse
                {
                    petName=item.petName,
                    age = age,
                    petId = item.petId,
                    birthDate = item.birthDate,
                    sex = item.sex.ToString(),
                    petType = petDetail.petType,
                    petClass = petDetail.petClass,
                    petBreed = petDetail.petBreed
                }
                );
            }
            return petsList;
        }

        public async Task<myPetResponse> GetUserPet(string owner, string petName, int petId)
        {
            var myPet = await _myPetRepository.GetOnePet(owner, petName, petId);

            DateOnly now = DateOnly.FromDateTime(DateTime.Now);
            int age = (now.DayNumber - myPet.birthDate.DayNumber)/365;
            var petDetail = await _petRepository.GetOnePet(myPet.petId);

            myPetResponse myPetResponse = new()
            {
                petName=myPet.petName,
                age = age,
                petId = myPet.petId,
                birthDate = myPet.birthDate,
                sex = myPet.sex.ToString(),
                petType = petDetail.petType,
                petClass = petDetail.petClass,
                petBreed = petDetail.petBreed
            };

            return myPetResponse;
        }

    }
}

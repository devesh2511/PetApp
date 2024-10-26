﻿using Amazon.Runtime.Internal;
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
    public class MyPetService
    {
        private MyPetRepository _myPetRepository;
        public MyPetService(MyPetRepository myPetRepository)
        {
            _myPetRepository = myPetRepository;
        }

        public async Task<dynamic> CreateMyPet(petRequest request)
        {
            var userPets = await _myPetRepository.GetUserPets(request.owner);
            var petExists = userPets.Find(x=>x.birthDate == request.birthDate && x.petType == request.petType.ToString() && x.petName == request.petName);
            if (petExists != null)
            {
                return ("Pet " + request.petName + " already exist!"); ;
            }

            var newPet = new myPetDB
            {
                owner = request.owner,
                petName = request.petName,
                birthDate = request.birthDate,
                petType = request.petType.ToString()
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
                petsList.Add(new myPetResponse
                {
                    petName=item.petName,
                    age = age,
                    petType = item.petType,
                    birthDate = item.birthDate,
                }
                );
            }
            return petsList;
        }
    }
}

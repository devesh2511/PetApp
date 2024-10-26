using PetApp.Utility.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using System.Security.Cryptography;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Xml.Linq;

namespace PetApp.Utility
{
    public class CompareStrings
    {
    
        public CompareStrings()
        {
        
        }
        public (bool, string, string) IsStringEqual(string a, string b)
        {
            string A = a.ToUpper().Trim();
            string B = b.ToUpper().Trim();

            if(A.Contains(B) || B.Contains(A))
            {
                return (true,A,B);
            }
            return (false,A,B);
        }
    }
}

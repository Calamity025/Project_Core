using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace Presentation
{
    public class AuthOptions
    {
        public const string ISSUER = "Calamity"; 
        public const string AUDIENCE = "http://localhost:44324/"; 
        const string KEY = "mysupersecret_secretkey!123";   
        public const int LIFETIME = 1; 
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}

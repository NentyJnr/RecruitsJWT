﻿using Microsoft.IdentityModel.Tokens;
using Recruits.API.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Recruits.API.Repository
{
    public class TokenRepository : ITokenRepository
    {

        Dictionary<string, string> UsersRecord = new Dictionary<string, string>
        {
            { "admin","admin"},
            { "password","password"}
        };


        private readonly IConfiguration _configuration;



        public TokenRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }



        public Tokens Authenticate(Users users)
        {
           if (!UsersRecord.Any(x => x.Key == users.Name && x.Value == users.Password))
            {
                return null;
            }

           //We have Authenticated
            //Generate JSON Web Token

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes(_configuration["JWT:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, users.Name)
                }),
                Expires = DateTime.UtcNow.AddMinutes(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return new Tokens { Token = tokenHandler.WriteToken(token) };
 
        }
    }
}

using Newtonsoft.Json;
using System;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Claustro.Domain;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Text;

namespace Claustro.Security
{
    public static class TokenHandler

    {
        public const string JWT_TOKEN_ISSUER = "http://www.uai.edu.ar";
        public const string JWT_TOKEN_AUDIENCE = "http://www.uai.edu.ar";


        // TODO:
        //pasar todo lo que se necesite storear en el token, claims
        public static string GenerateToken(User user)
        {

            //};

            var claims = new Claim[]
            {
                new Claim("name", user.Username),
                new Claim("rol", user.Rol.ToString()),
                new Claim("nameidentifier", user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Mail),
                new Claim(ClaimTypes.Role, user.Rol.ToString())
            };

            var key = Encoding.UTF8.GetBytes(Convert.ToBase64String(Encoding.UTF8.GetBytes("my super secret key goes here")));
            var jwt = new JwtSecurityToken(
                issuer: JWT_TOKEN_ISSUER,
                audience: JWT_TOKEN_AUDIENCE,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), "HS256"));




            var tokenString = new JwtSecurityTokenHandler().WriteToken(jwt);


            return tokenString;
        }

    }

}

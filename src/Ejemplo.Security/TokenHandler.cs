using Newtonsoft.Json;
using System;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Collections.Generic;
using Ejemplo.Entities;

namespace Ejemplo.Security
{
    public static class TokenHandler

    {
        public const string JWT_TOKEN_ISSUER = "http://case.uai.edu.ar";
        public const string JWT_TOKEN_AUDIENCE = "http://case.uai.edu.ar";


        // TODO:
        //pasar todo lo que se necesite storear en el token, claims
        public static string GenerateToken(User user)
        {
           
            var claims = new Claim[]
            {
                new Claim("name", user.Username),
                new Claim("nameidentifier", user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Mail),
                 new Claim(ClaimTypes.Role, "admin")
            };


            var jwt = new JwtSecurityToken(
                issuer: JWT_TOKEN_ISSUER,
                audience: JWT_TOKEN_AUDIENCE,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: new SigningCredentials(Keys.RSAKey, SecurityAlgorithms.RsaSha256Signature));

            var tokenString = new JwtSecurityTokenHandler().WriteToken(jwt);


            return tokenString;
        }

    }
}

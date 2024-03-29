﻿using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace APBD_09.Helpers
{
    public class AuthHelper
    {
        public static JwtSecurityToken GenerateToken(/* obiekt użytkownika z bazy danych */ string secret)
        {
            //Claimsy wypełniamy operując na properties z obiektu użytkownika z DB
            Claim[] claims =
            {
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim(ClaimTypes.Name, "Sylwia"),
                new Claim(ClaimTypes.Surname, "Stern"),
                new Claim(ClaimTypes.Email, "ss@pjwstk.edu.pl"),
                new Claim(ClaimTypes.Role, "user"),
                new Claim(ClaimTypes.Role, "student")
            };

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            /*kto wystawia token
             * dla kogo wystawiony token
             * claims
             * kiedy token ma wygasnąć
             */

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: "http://localhost:5000", 
                audience: "http://localhost:5002",
                claims: claims,
                expires: DateTime.Now.AddMinutes(5),
                signingCredentials: creds
                );

            return token;
        }
    }
}

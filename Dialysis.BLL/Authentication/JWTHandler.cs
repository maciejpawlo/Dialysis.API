using Dialysis.BE.Authentication;
using Dialysis.DAL;
using Dialysis.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Dialysis.BLL.Authentication
{
    public class JWTHandler : IJWTHandler
    {
        private readonly IConfiguration config;
        private readonly DialysisContext context;

        public JWTHandler(IConfiguration config, DialysisContext context)
        {
            this.config = config;
            this.context = context;
        }

        public JwtResponse GenerateTokens(string userId, Claim[] claims, DateTime date)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Tokens:Key"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
              config["Tokens:Issuer"],
              config["Tokens:Audience"],
              claims,
              expires: DateTime.UtcNow.AddMinutes(30),
              signingCredentials: creds);

            var refreshToken = new DAL.Entities.RefreshToken 
            {
                Token = GenerateRefreshTokenString(),
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                IsUsed = false,
                UserId = userId,
            };

            context.RefreshTokens.Add(refreshToken);
            context.SaveChanges();

            var result = new JwtResponse
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = refreshToken,
                UserId = userId,    
            };

            return result;
        }

        public JwtResponse RefreshToken(RefreshToken token, Claim[] claims, DateTime date)
        {
            //Check in db if refresh token exists
            //if exists AND is not expired/used then generate new access token AND revoke old token
            //if not then user has to authenticate again

            if (token == null)
            {
                return null;
            }

            if (token.ExpiresAt < DateTime.UtcNow)
            {
                return null;
            }

            if (token.IsUsed == true)
            {
                return null;
            }

            var newTokens = GenerateTokens(token.UserId, claims, DateTime.UtcNow);
            token.IsUsed = true;
            context.SaveChanges();

            return newTokens;
        }

        private string GenerateRefreshTokenString()
        {
            var randomNumber = new byte[32];
            using var randomNumberGenerator = RandomNumberGenerator.Create();
            randomNumberGenerator.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}

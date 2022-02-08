using Dialysis.BE.Authentication;
using Dialysis.DAL;
using Dialysis.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Dialysis.BLL.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IJWTHandler jwtHandler;
        private readonly SignInManager<User> signInManager;
        private readonly UserManager<User> userManager;
        private readonly DialysisContext context;

        public AuthenticationService(IJWTHandler jwtHandler, SignInManager<User> signInManager, UserManager<User> userManager, DialysisContext context)
        {
            this.jwtHandler = jwtHandler;
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.context = context;
        }

        public async Task<AuthenticateResponse> AuthenticateAsync(AuthenticateRequest authenticateRequest)
        {
            var user = await userManager.FindByNameAsync(authenticateRequest.UserName);
            var result = await signInManager.CheckPasswordSignInAsync(user, authenticateRequest.Password, false);

            if (result.Succeeded)
            {
                var role = await userManager.GetRolesAsync(user);
                var userId = await userManager.GetUserIdAsync(user);

                var claims = new[]
                {
                  new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                  new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                  new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                  new Claim(ClaimTypes.Role, role.FirstOrDefault()),
                };
                var jwtResult = jwtHandler.GenerateTokens(userId, claims, DateTime.UtcNow);

                return new AuthenticateResponse 
                { 
                    AccessToken = jwtResult.AccessToken, 
                    RefreshToken = jwtResult.RefreshToken.Token,
                    UserName = authenticateRequest.UserName 
                };
            }

            return null;
        }

        public async Task<AuthenticateResponse> ResfreshTokenAsync(RefreshTokenRequest refreshTokenRequest)
        {
            var refreshToken = context.RefreshTokens
                .Where(x => x.Token == refreshTokenRequest.RefreshToken).FirstOrDefault();
            if (refreshToken == null)
                return null;

            var jwtResult = jwtHandler.RefreshToken(refreshToken.UserId, refreshTokenRequest.AccessToken, refreshTokenRequest.RefreshToken, DateTime.UtcNow);

            return new AuthenticateResponse
            {
                AccessToken = jwtResult.AccessToken,
                RefreshToken = jwtResult.RefreshToken.Token,
            };
        }
    }
}

using Dialysis.BE.Authentication;
using Dialysis.BE.Helpers;
using Dialysis.DAL;
using Dialysis.DAL.Entities;
using Microsoft.AspNetCore.Http;
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
            var response = new AuthenticateResponse();
            var user = await userManager.FindByNameAsync(authenticateRequest.UserName);
            var result = await signInManager.CheckPasswordSignInAsync(user, authenticateRequest.Password, false);

            if (result.Succeeded)
            {
                var role = await userManager.GetRolesAsync(user);
                var userId = await userManager.GetUserIdAsync(user);

                var claims = new[]
                {
                  //new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                  new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                  new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                  new Claim(ClaimTypes.Role, role.FirstOrDefault()),
                };
                var jwtResult = jwtHandler.GenerateTokens(userId, claims, DateTime.UtcNow);
                
                response.IsSuccessful = true;
                response.AccessToken = jwtResult.AccessToken;
                response.UserName = authenticateRequest.UserName;
                return response;
            }
            response.StatusCode = StatusCodes.Status401Unauthorized;
            response.IsSuccessful = false;
            return response;
        }

        public async Task<BaseResponse> ChangePassword(ChangePasswordRequest resetPasswordRequest, string userName)
        {
            var response = new BaseResponse();
            var user = await userManager.FindByNameAsync(userName);
            var changePasswordResult = await userManager.ChangePasswordAsync(user, resetPasswordRequest.OldPassword, resetPasswordRequest.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                response.StatusCode = StatusCodes.Status401Unauthorized;
                return response;
            }
            response.StatusCode = StatusCodes.Status200OK;
            return response;
        }

        public async Task<AuthenticateResponse> ResfreshTokenAsync(RefreshTokenRequest refreshTokenRequest)
        {
            var response = new AuthenticateResponse();

            var refreshToken = context.RefreshTokens
                .Where(x => x.Token == refreshTokenRequest.RefreshToken).FirstOrDefault();
            if (refreshToken == null)
            {
                response.IsSuccessful = false;
                response.StatusCode = StatusCodes.Status404NotFound;
                response.Message = "Refresh token invalid";
                return response;
            }

            var user = await userManager.FindByIdAsync(refreshToken.UserId);
            var role = await userManager.GetRolesAsync(user);

            var claims = new[]
            {
                  //new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                  new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                  new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                  new Claim(ClaimTypes.Role, role.FirstOrDefault()),
            };

            var jwtResult = jwtHandler.RefreshToken(refreshToken, claims, DateTime.UtcNow);

            response.AccessToken = jwtResult.AccessToken;
            response.RefreshToken = jwtResult?.RefreshToken.Token;
            response.StatusCode = StatusCodes.Status200OK;
            return response;
        }

        public async Task<BaseResponse> SetFirstPasswordAsync(SetFirstPasswordRequest request, string userName)
        {
            var response = new BaseResponse();
            var user = await userManager.FindByNameAsync(userName);
            if (user == null)
            {
                response.StatusCode = StatusCodes.Status404NotFound;
                response.Message = "Could not find user with matching username";
                response.IsSuccessful = false;
            }

            if (string.IsNullOrEmpty(user.Email))
            {
                response.StatusCode = StatusCodes.Status400BadRequest;
                response.Message = "User had already set the password";
                response.IsSuccessful = false;
            }

            var setEmailResult = await userManager.SetEmailAsync(user, request.Email);
            if (!setEmailResult.Succeeded)
            {
                response.StatusCode = StatusCodes.Status500InternalServerError;
                response.Message = "Could set user's email";
                response.IsSuccessful = false;
            }

            response.StatusCode = StatusCodes.Status200OK;
            response.IsSuccessful = true;
            return response;
        }
    }
}

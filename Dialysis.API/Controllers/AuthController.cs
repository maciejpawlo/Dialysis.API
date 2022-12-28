using Dialysis.BE.Authentication;
using Dialysis.BE.Helpers;
using Dialysis.BLL.Authentication;
using Dialysis.DAL;
using Dialysis.DAL.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IAuthenticationService = Dialysis.BLL.Authentication.IAuthenticationService;

namespace Dialysis.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService authenticationService;

        public AuthController(IAuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<AuthenticateResponse>> Authenticate(AuthenticateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var authResult = await authenticationService.AuthenticateAsync(request);
            return StatusCode(authResult.StatusCode, authResult);
        }

        //[Authorize]
        [HttpPost("refreshToken")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<AuthenticateResponse>> RefreshToken(RefreshTokenRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var refreshResult = await authenticationService.RefreshTokenAsync(request);
            return StatusCode(refreshResult.StatusCode, refreshResult);
        }

        [Authorize]
        [HttpPost("changePassword")]
        public async Task<ActionResult<BaseResponse>> ChangePassword(ChangePasswordRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var changePasswordResult = await authenticationService.ChangePassword(request, HttpContext.User.Identity.Name);
            return StatusCode(changePasswordResult.StatusCode, changePasswordResult);
        }

        [Authorize]
        [HttpPost("setFirstPassword")]
        public async Task<ActionResult<BaseResponse>> SetFirstPassword(SetFirstPasswordRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var setFirstPasswordResult = await authenticationService.SetFirstPasswordAsync(request, HttpContext.User.Identity.Name);
            return StatusCode(setFirstPasswordResult.StatusCode, setFirstPasswordResult);
        }

        [Authorize(Roles = $"{Role.Admin}, {Role.Doctor}")]
        [HttpGet("test")]
        public async Task<IActionResult> Test()
        {
            return Ok();
        }
    }
}

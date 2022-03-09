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
        public async Task<ActionResult<AuthenticateResponse>> Authenticate(AuthenticateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var authResult = await authenticationService.AuthenticateAsync(request);
            if (authResult == null)
            {
                return Unauthorized();
            }

            return Ok(authResult);
        }

        //[Authorize]
        [HttpPost("refreshToken")]
        public async Task<ActionResult<AuthenticateResponse>> RefreshToken(RefreshTokenRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var refreshResult = await authenticationService.ResfreshTokenAsync(request);
            if (refreshResult == null)
            {
                return Unauthorized();
            }

            return Ok(refreshResult);
        }

        [Authorize(Roles = $"{Role.Admin}, {Role.Doctor}")]
        [HttpGet("test")]
        public async Task<IActionResult> Test()
        {
            return Ok();
        }
    }
}

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
        private readonly SignInManager<User> signInManager;
        private readonly UserManager<User> userManager;
        private readonly IConfiguration config;
        private readonly DialysisContext context;
        private readonly IAuthenticationService authenticationService;

        public AuthController(SignInManager<User> signInManager, 
            UserManager<User> userManager, IConfiguration config, 
            DialysisContext context, IAuthenticationService authenticationService)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.config = config;
            this.context = context;
            this.authenticationService = authenticationService;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<ActionResult<AuthenticateResponse>> Authenticate([FromBody] AuthenticateRequest request)
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

        //NOTE: For test reasons only
        [Authorize(Roles = Role.Admin)]
        [HttpPost("create")]
        public async Task<IActionResult> CreateUser(string userName, string password)
        {
            var existingUser = await userManager.FindByNameAsync(userName);
            if (existingUser != null)
            {
                return BadRequest("User already exists!");
            }

            var user = new Doctor() { UserName = userName, PermissionNumber = 1111111 };

            await userManager.CreateAsync(user, password);
            await context.Doctors.AddAsync(user);
            await context.SaveChangesAsync();
            return Ok();
        }

        [Authorize(Roles = Role.Admin)]
        [HttpGet("test")]
        public async Task<IActionResult> Test()
        {
            return Ok();
        }
    }
}

using Dialysis.BE.Users;
using Dialysis.BE.Helpers;
using Dialysis.DAL;
using Dialysis.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using IdentityPasswordGenerator;
using Microsoft.Extensions.Options;
using Dialysis.BLL.Users;

namespace Dialysis.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [Authorize(Roles = Role.Admin)]
        [HttpPost("createDoctor")]
        public async Task<ActionResult<CreateDoctorResponse>> CreateDoctor(CreateDoctorRequest request)
        {
            var response = await userService.CreateDoctorAsync(request);
            if (!response.IsSuccessful)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [Authorize(Roles = $"{Role.Admin}, {Role.Doctor}")]
        [HttpPost("createPatient")]
        public async Task<ActionResult<CreatePatientResponse>> CreatePatient(CreatePatientRequest request)
        {
            var response = await userService.CreatePatientAsync(request);
            if (!response.IsSuccessful)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}

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
        [HttpPost("doctors")]
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
        [HttpPost("patients")]
        public async Task<ActionResult<CreatePatientResponse>> CreatePatient(CreatePatientRequest request)
        {
            var response = await userService.CreatePatientAsync(request);
            if (!response.IsSuccessful)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        //[Authorize(Roles = $"{Role.Admin}, {Role.Doctor}")]
        [HttpPost("assignPatientToDoctor")]
        public async Task<IActionResult> AssignPatientToDoctor(AssignPatientToDoctorRequest request)
        {
            var response = await userService.AssignPatientToDoctorAsync(request);
            if (!response.IsSuccessful)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        //[Authorize(Roles = $"{Role.Admin}, {Role.Doctor}")]
        [HttpPost("unassignPatientFromDoctor")]
        public async Task<IActionResult> UnassignPatientFromDoctor(AssignPatientToDoctorRequest request)
        {
            var response = await userService.UnassignPatientFromDoctorAsync(request);
            if (!response.IsSuccessful)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [Authorize(Roles = Role.Admin)]
        [HttpDelete("doctors/{id:int}")]
        public async Task<IActionResult> DeleteDoctor(int id)
        {
            return Ok();
        }

        [Authorize(Roles = $"{Role.Admin}, {Role.Doctor}")]
        [HttpDelete("patients/{id:int}")]
        public async Task<IActionResult> DeletePatient(int id)
        {
            return Ok();
        }

        [Authorize(Roles = $"{Role.Admin}, {Role.Doctor}")]
        [HttpPut("doctors/{id:int}")]
        public async Task<IActionResult> EditDoctor(int id)
        {
            return Ok();
        }

        [Authorize(Roles = $"{Role.Admin}, {Role.Doctor}")]
        [HttpPut("patients/{id:int}")]
        public async Task<IActionResult> EditPatient(int id)
        {
            return Ok();
        }

        [Authorize(Roles = $"{Role.Admin}, {Role.Doctor}")]
        [HttpGet("doctors")]
        public async Task<IActionResult> GetDoctors()
        {
            return Ok();
        }

        [Authorize(Roles = $"{Role.Admin}, {Role.Doctor}")]
        [HttpGet("patients")]
        public async Task<IActionResult> GetPatients()
        {
            return Ok();
        }
    }
}

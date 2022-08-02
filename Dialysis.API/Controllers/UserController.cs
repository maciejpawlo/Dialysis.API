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
using Dialysis.DAL.DTOs;
using Dialysis.DAL.Helpers.Enums;
using System;
using System.Linq;
using Microsoft.AspNetCore.Http;

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
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<CreateUserResponse>> CreateDoctor(CreateDoctorRequest request)
        {
            var response = await userService.CreateDoctorAsync(request);
            return StatusCode(response.StatusCode, response);
        }

        [Authorize(Roles = $"{Role.Admin}, {Role.Doctor}")]
        [HttpPost("patients")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<CreateUserResponse>> CreatePatient(CreatePatientRequest request)
        {
            var response = await userService.CreatePatientAsync(request);
            return StatusCode(response.StatusCode, response);
        }

        [Authorize(Roles = $"{Role.Admin}, {Role.Doctor}")]
        [HttpPost("assignPatientToDoctor")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<BaseResponse>> AssignPatientToDoctor(AssignPatientToDoctorRequest request)
        {
            var response = await userService.AssignPatientToDoctorAsync(request);
            return StatusCode(response.StatusCode, response);
        }

        [Authorize(Roles = $"{Role.Admin}, {Role.Doctor}")]
        [HttpPost("unassignPatientFromDoctor")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<BaseResponse>> UnassignPatientFromDoctor(AssignPatientToDoctorRequest request)
        {
            var response = await userService.UnassignPatientFromDoctorAsync(request);
            return StatusCode(response.StatusCode, response);
        }

        [Authorize(Roles = Role.Admin)]
        [HttpDelete("doctors/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<BaseResponse>> DeleteDoctor(int id)
        {
            var resposne = await userService.DeleteDoctor(id);
            return StatusCode(resposne.StatusCode, resposne);
        }

        [Authorize(Roles = $"{Role.Admin}, {Role.Doctor}")]
        [HttpDelete("patients/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<BaseResponse>> DeletePatient(int id)
        {
            var response = await userService.DeletePatient(id);
            return StatusCode(response.StatusCode, response);
        }

        [Authorize(Roles = $"{Role.Admin}, {Role.Doctor}")]
        [HttpPut("doctors/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<BaseResponse>> EditDoctor(int id, [FromBody] DoctorDTO doctorDTO)
        {
            var response = await userService.EditDoctor(id, doctorDTO);
            return StatusCode(response.StatusCode, response);
        }

        [Authorize(Roles = $"{Role.Admin}, {Role.Doctor}")]
        [HttpPut("patients/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<BaseResponse>> EditPatient(int id, [FromBody] PatientDTO patientDTO)
        {
            var response = await userService.EditPatient(id, patientDTO);
            return StatusCode(response.StatusCode, response);
        }

        [Authorize(Roles = $"{Role.Admin}, {Role.Doctor}")]
        [HttpGet("doctors")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<GetDoctorsResponse>> GetDoctors(string firstName, string lastName, string permissionNumber, int? patientID, bool includePatients = false)
        {
            var response = await userService.GetDoctors(includePatients, x => (firstName == null || x.FirstName.Contains(firstName))
            && (lastName == null || x.LastName.Contains(lastName))
            && (permissionNumber == null || x.PermissionNumber.Contains(permissionNumber))
            && (patientID == null || x.Patients.Any(p => p.PatientID == patientID)));

            return StatusCode(response.StatusCode, response);
        }

        [Authorize(Roles = $"{Role.Admin}, {Role.Doctor}")]
        [HttpGet("patients")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<GetPatientsResponse>> GetPatients(string firstName, string lastName, string pesel, string gender, int? doctorID, bool includeDoctors = false)
        {
            var response = await userService.GetPatients(includeDoctors, x => (firstName == null || x.FirstName.Contains(firstName))
            && (lastName == null || x.LastName.Contains(lastName))
            && (pesel == null || x.PESEL == pesel)
            && (gender == null || x.Gender == (Gender)int.Parse(gender))
            && (doctorID == null || x.Doctors.Any(d => d.DoctorID == doctorID)));

            return StatusCode(response.StatusCode, response);
        }

        [Authorize(Roles = $"{Role.Admin}, {Role.Doctor}")]
        [HttpGet("doctors/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<GetDoctorsResponse>> GetDoctorByID(int id)
        {
            var response = await userService.GetDoctors(true, x => x.DoctorID == id);
            return StatusCode(response.StatusCode, response);
        }

        [Authorize(Roles = $"{Role.Admin}, {Role.Doctor}")]
        [HttpGet("patients/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<GetPatientsResponse>> GetPatientByID(int id)
        {
            var response = await userService.GetPatients(true, x => x.PatientID == id);
            return StatusCode(response.StatusCode, response);
        }

        [Authorize(Role.Admin)]
        [HttpPost("resetUsersPassword")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<BaseResponse>> ResetUsersPassword(ResetUsersPasswordRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var response = await userService.ResetUsersPassword(request);
            return StatusCode(response.StatusCode, response);
        }

        [Authorize]
        [HttpGet("UserInfo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<GetUserInfoResponse>> GetUserInfo()
        {
            var name = HttpContext.User.Identity.Name;
            var response = await userService.GetUserInfo(name);
            return StatusCode(response.StatusCode, response);
        }
    }
}

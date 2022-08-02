using AutoMapper;
using Dialysis.BE.Helpers;
using Dialysis.BE.Users;
using Dialysis.DAL;
using Dialysis.DAL.DTOs;
using Dialysis.DAL.Entities;
using IdentityPasswordGenerator;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Dialysis.BLL.Users
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> userManager;
        private readonly DialysisContext context;
        private readonly IdentityOptions identityOptions;
        private readonly IMapper mapper;
        private readonly IPasswordGenerator passwordGenerator;

        public UserService(UserManager<User> userManager,
            DialysisContext context,
            IOptions<IdentityOptions> identityOptions,
            IMapper mapper, IPasswordGenerator passwordGenerator)
        {
            this.userManager = userManager;
            this.context = context;
            this.mapper = mapper;
            this.identityOptions = identityOptions?.Value;
            this.passwordGenerator = passwordGenerator;
        }

        public async Task<CreateUserResponse> CreateDoctorAsync(CreateDoctorRequest request)
        {
            var existingUser = await userManager.FindByNameAsync(request.UserName);
            if (existingUser != null)
            {
                return new CreateUserResponse { IsSuccessful = false, Message = "User already exists!", StatusCode = StatusCodes.Status400BadRequest };
            }

            var user = new User()
            {
                FirstName = request.Firstname,
                LastName = request.Lastname,
                UserName = request.UserName
            };

            var password = passwordGenerator.GeneratePassword(identityOptions.Password);

            await userManager.CreateAsync(user, password);
            await userManager.AddToRoleAsync(user, Role.Doctor);

            var addedUser = await userManager.FindByNameAsync(request.UserName);
            var doctor = new Doctor()
            {
                PermissionNumber = request.PermissionNumber,
                FirstName = request.Firstname,
                LastName = request.Lastname,
                UserID = addedUser.Id
            };

            await context.Doctors.AddAsync(doctor);
            await context.SaveChangesAsync();
            return new CreateUserResponse { Password = password, UserName = request.UserName, IsSuccessful = true, StatusCode = StatusCodes.Status201Created };
        }

        public async Task<CreateUserResponse> CreatePatientAsync(CreatePatientRequest request)
        {
            var existingUser = await userManager.FindByNameAsync(request.UserName);
            if (existingUser != null)
            {
                return new CreateUserResponse { IsSuccessful = false, Message = "User already exists!", StatusCode = StatusCodes.Status400BadRequest };
            }

            var user = new User()
            {
                FirstName = request.Firstname,
                LastName = request.Lastname,
                UserName = request.UserName
            };

            var password = passwordGenerator.GeneratePassword(identityOptions.Password);

            await userManager.CreateAsync(user, password);
            await userManager.AddToRoleAsync(user, Role.Patient);

            var addedUser = await userManager.FindByNameAsync(request.UserName);
            var patient = new Patient()
            {
                PESEL = request.PESEL,
                FirstName = request.Firstname,
                LastName = request.Lastname,
                BirthDate = request.BirthDate,
                Gender = request.Gender,
                UserID = addedUser.Id
            };

            await context.Patients.AddAsync(patient);
            await context.SaveChangesAsync();
            return new CreateUserResponse { Password = password, UserName = request.UserName, IsSuccessful = true, StatusCode = StatusCodes.Status201Created };
        }

        public async Task<BaseResponse> AssignPatientToDoctorAsync(AssignPatientToDoctorRequest request)
        {
            var patient = context.Patients
                .Where(p => p.PatientID == request.PatientID)
                .FirstOrDefault();

            var doctor = context.Doctors
                .Where(d => d.DoctorID == request.DoctorID)
                .FirstOrDefault();

            if (patient == null)
                return new BaseResponse { IsSuccessful = false, Message = "Paitent with given id does not exist.", StatusCode = StatusCodes.Status404NotFound };

            if (doctor == null)
                return new BaseResponse { IsSuccessful = false, Message = "Doctor with given id does not exist.", StatusCode = StatusCodes.Status404NotFound };
            try
            {
                doctor.Patients.Add(patient);
                await context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return new BaseResponse
                {
                    IsSuccessful = false,
                    Message = $"An error was thrown while trying to assign patient to doctor: {e}",
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }

            return new BaseResponse { IsSuccessful = true, StatusCode = StatusCodes.Status200OK };
        }

        public async Task<BaseResponse> UnassignPatientFromDoctorAsync(AssignPatientToDoctorRequest request)
        {
            var patient = context.Patients
                .Where(p => p.PatientID == request.PatientID)
                .FirstOrDefault();

            var doctor = context.Doctors
                .Where(d => d.DoctorID == request.DoctorID)
                .FirstOrDefault();

            if (patient == null)
                return new BaseResponse { IsSuccessful = false, Message = "Paitent with given id does not exist.", StatusCode = StatusCodes.Status404NotFound };

            if (doctor == null)
                return new BaseResponse { IsSuccessful = false, Message = "Doctor with given id does not exist.", StatusCode = StatusCodes.Status404NotFound };

            try
            {
                await context.Entry(doctor).Collection("Patients").LoadAsync();
                doctor.Patients.Remove(patient);
                await context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return new BaseResponse
                {
                    IsSuccessful = false,
                    Message = $"An error was thrown while trying to unassign patient from doctor: {e.Message}",
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }

            return new BaseResponse { IsSuccessful = true, StatusCode = StatusCodes.Status200OK };
        }

        public async Task<BaseResponse> DeleteDoctor(int id)
        {
            var doctorToDelete = context.Doctors
                .Where(d => d.DoctorID == id)
                .FirstOrDefault();

            var userToDelete = await userManager.FindByIdAsync(doctorToDelete?.UserID);
            if (userToDelete == null || doctorToDelete == null)
            {
                return new BaseResponse
                {
                    IsSuccessful = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "Could not delete doctor, no doctor with given id was found."
                };
            }

            var userDeleteResult = await userManager.DeleteAsync(userToDelete);
            context.Doctors.Remove(doctorToDelete);

            if (!userDeleteResult.Succeeded)
            {
                return new BaseResponse
                {
                    IsSuccessful = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "Could not delete doctor."
                };
            }
            await context.SaveChangesAsync();

            return new BaseResponse { IsSuccessful = true, StatusCode = StatusCodes.Status200OK };
        }

        public async Task<BaseResponse> DeletePatient(int id)
        {
            var patientToDelete = context.Patients
                .Where(p => p.PatientID == id)
                .FirstOrDefault();

            var userToDelete = await userManager.FindByIdAsync(patientToDelete?.UserID);
            if (userToDelete == null || patientToDelete == null)
            {
                return new BaseResponse
                {
                    IsSuccessful = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "Could not delete doctor, no doctor with given id was found."
                };
            }

            var userDeleteResult = await userManager.DeleteAsync(userToDelete);
            context.Patients.Remove(patientToDelete);

            if (!userDeleteResult.Succeeded)
            {
                return new BaseResponse
                {
                    IsSuccessful = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "Could not delete doctor."
                };
            }
            await context.SaveChangesAsync();

            return new BaseResponse { IsSuccessful = true, StatusCode = StatusCodes.Status200OK };
        }

        public async Task<BaseResponse> EditDoctor(int id, DoctorDTO doctorDTO)
        {
            var oldDoctor = context.Doctors
                .Where(d => d.DoctorID == id)
                .FirstOrDefault();

            if (oldDoctor == null)
            {
                return new BaseResponse { IsSuccessful = false, StatusCode = StatusCodes.Status404NotFound };
            }

            mapper.Map(doctorDTO, oldDoctor);

            await context.SaveChangesAsync();

            return new BaseResponse { IsSuccessful = true, StatusCode = StatusCodes.Status200OK };
        }

        public async Task<BaseResponse> EditPatient(int id, PatientDTO patientDTO)
        {
            var oldPatient = context.Patients
                .Where(d => d.PatientID == id)
                .FirstOrDefault();

            if (oldPatient == null)
            {
                return new BaseResponse { IsSuccessful = false, StatusCode = StatusCodes.Status404NotFound };
            }

            mapper.Map(patientDTO, oldPatient);

            await context.SaveChangesAsync();

            return new BaseResponse { IsSuccessful = true, StatusCode = StatusCodes.Status200OK };
        }

        public async Task<GetDoctorsResponse> GetDoctors(bool includePatients, Func<Doctor, bool> filter = null)
        {
            var doctors = context.Doctors
                .Include(x => x.Patients)
                .AsNoTracking()
                .Where(filter ?? (x => true)).ToList();

            if (doctors.Count == 0)
            {
                return new GetDoctorsResponse { StatusCode = StatusCodes.Status404NotFound };
            }

            var result = mapper.Map<IEnumerable<DoctorDTO>>(doctors);

            return new GetDoctorsResponse { StatusCode = StatusCodes.Status200OK, Doctors = result };
        }

        public async Task<GetPatientsResponse> GetPatients(bool includeDoctors, Func<Patient, bool> filter = null)
        {
            var patients = context.Patients
                .Include(x => x.Doctors)
                .AsNoTracking()
                .Where(filter ?? (p => true)).ToList();

            if (patients.Count == 0)
            {
                return new GetPatientsResponse { StatusCode = StatusCodes.Status404NotFound };
            }

            var result = mapper.Map<IEnumerable<PatientDTO>>(patients);

            return new GetPatientsResponse { StatusCode = StatusCodes.Status200OK, Patients = result };
        }

        public async Task<CreateUserResponse> ResetUsersPassword(ResetUsersPasswordRequest request)
        {
            var response = new CreateUserResponse();
            var user = await userManager.FindByNameAsync(request.UserName);
            if (user == null)
            {
                response.StatusCode = StatusCodes.Status404NotFound;
                response.Message = "Could not find user with matching username";
                response.IsSuccessful = false;
            }

            var newPassword = passwordGenerator.GeneratePassword(identityOptions.Password);
            await userManager.RemovePasswordAsync(user);
            await userManager.AddPasswordAsync(user, newPassword);

            response.StatusCode = StatusCodes.Status200OK;
            response.IsSuccessful = true;
            response.UserName = request.UserName;
            response.Password = newPassword;
            return response;
        }

        public async Task<GetUserInfoResponse> GetUserInfo(string userName)
        {
            var response = new GetUserInfoResponse();
            var user = await userManager.FindByNameAsync(userName);
            if (user == null)
            {
                response.StatusCode = StatusCodes.Status404NotFound;
                response.Message = "Could not find user with matching username";
            }

            var role = (await userManager.GetRolesAsync(user)).First();

            response.UserName = user.UserName;
            response.Role = role;
            response.FirstName = user.FirstName;
            response.LastName = user.LastName;

            switch (role)
            {
                case Role.Doctor:
                    var doctorInfo = await GetDoctors(false, x => x.UserID == user.Id);
                    response.InternalUserID = doctorInfo.Doctors.FirstOrDefault().DoctorID;
                    break;

                case Role.Patient:
                    var patientInfo = await GetPatients(false, x => x.UserID == user.Id);
                    response.InternalUserID = patientInfo.Patients.FirstOrDefault().PatientID;
                    break;

                default:
                    break;
            }
            response.StatusCode = StatusCodes.Status200OK;
            return response;
        }
    }
}

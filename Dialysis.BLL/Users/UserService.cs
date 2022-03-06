using Dialysis.BE.Helpers;
using Dialysis.BE.Users;
using Dialysis.DAL;
using Dialysis.DAL.Entities;
using IdentityPasswordGenerator;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dialysis.BLL.Users
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> userManager;
        private readonly DialysisContext context;
        private readonly IdentityOptions identityOptions;

        public UserService(UserManager<User> userManager, 
            DialysisContext context,
            IOptions<IdentityOptions> identityOptions)
        {
            this.userManager = userManager;
            this.context = context;
            this.identityOptions = identityOptions?.Value;
        }

        public async Task<CreateDoctorResponse> CreateDoctorAsync(CreateDoctorRequest request)
        {
            var existingUser = await userManager.FindByNameAsync(request.UserName);
            if (existingUser != null)
            {
                return new CreateDoctorResponse { IsSuccessful = false, Message = "User already exists!" };
            }

            var user = new User() 
            {
                FirstName = request.Firstname,
                LastName = request.Lastname,
                UserName = request.UserName
            };

            var passwordGenerator = new PasswordGenerator();
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
            return new CreateDoctorResponse { Password = password, UserName = request.UserName, IsSuccessful = true };
        }

        public async Task<CreatePatientResponse> CreatePatientAsync(CreatePatientRequest request)
        {
            var existingUser = await userManager.FindByNameAsync(request.UserName);
            if (existingUser != null)
            {
                return new CreatePatientResponse { IsSuccessful = false, Message = "User already exists!" };
            }
            //TODO: PESEL validation
            var user = new User()
            {
                FirstName = request.Firstname,
                LastName = request.Lastname,
                UserName = request.UserName
            };

            var passwordGenerator = new PasswordGenerator();
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
            return new CreatePatientResponse { Password = password, UserName = request.UserName, IsSuccessful = true };
        }
    }
}

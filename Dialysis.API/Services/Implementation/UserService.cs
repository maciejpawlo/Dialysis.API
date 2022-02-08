using Dialysis.DAL;
using Dialysis.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dialysis.API.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly SignInManager<User> signInManager;
        private readonly UserManager<User> userManager;
        private readonly IConfiguration config;
        private readonly DialysisContext context;

        public UserService(SignInManager<User> signInManager, UserManager<User> userManager, IConfiguration config, DialysisContext context)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.config = config;
            this.context = context;
        }

        private async Task CreateUserAsync()
        {

        }

        public async Task<IdentityResult> CreateDoctorAsync(string userName, string password, Doctor doctor)
        {
            IdentityResult user = null;
            var existingUser = await userManager.FindByNameAsync(userName);
            if (existingUser != null)
            {
                return user;
            }

            user = await userManager.CreateAsync(doctor, password);
            await context.Doctors.AddAsync(doctor);
            await context.SaveChangesAsync();
            return user;
        }

        public async Task<IdentityResult> CreatePatientAsync(string userName, string password, Patient patient)
        {
            IdentityResult user = null;
            var existingUser = await userManager.FindByNameAsync(userName);
            if (existingUser != null)
            {
                return user;
            }

            user = await userManager.CreateAsync(patient, password);
            await context.Patients.AddAsync(patient);
            await context.SaveChangesAsync();
            return user;
        }
    }
}

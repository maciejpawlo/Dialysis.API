using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dialysis.DAL.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;

namespace Dialysis.DAL
{
    public class DialysisSeeder
    {
        private readonly DialysisContext context;
        private readonly IWebHostEnvironment hosting;
        private readonly UserManager<User> userManager;
        private RoleManager<IdentityRole> roleManager;

        public DialysisSeeder(DialysisContext context, IWebHostEnvironment hosting, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.context = context;
            this.hosting = hosting;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public async Task SeedAsync()
        {
            context.Database.EnsureCreated();

            var user = await userManager.FindByEmailAsync("maciejpawlo@gmail.com");

            if (user == null)
            {
                //Create admin
                user = new User() { UserName = "admin", FirstName = "admin", LastName = "admin", Email = "maciejpawlo@gmail.com" };

                var result = await userManager.CreateAsync(user, "P@ssw0rd!");

                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create admin user in Seeder");
                }

                //Create roles
                var roles = new[] { "Administrator", "Doctor", "Patient" };

                foreach (var role in roles)
                {
                    var roleResult = await roleManager.CreateAsync(new IdentityRole(role));
                    if (roleResult != IdentityResult.Success)
                    {
                        throw new InvalidOperationException($"Could not create role {role} in Seeder");
                    }
                }

                //Assign role to admin
                var addRoleResult = await userManager.AddToRoleAsync(user, "Administrator");
                if (addRoleResult != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not assign admin role in Seeder");
                }
            }

            context.SaveChanges();
        }
    }
}

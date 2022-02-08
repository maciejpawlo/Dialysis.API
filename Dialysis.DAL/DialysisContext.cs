using Dialysis.DAL.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace Dialysis.DAL
{
    public class DialysisContext : IdentityDbContext<User>
    {
        public DialysisContext(DbContextOptions<DialysisContext> options) : base(options)
        {

        }

        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Examination> Examinations { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}

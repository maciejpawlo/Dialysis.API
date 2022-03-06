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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //one to one relationship between User and Doctor, User and Patient
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasOne(a => a.Patient)
                .WithOne(b => b.User)
                .HasForeignKey<Patient>(e => e.UserID);

            modelBuilder.Entity<User>()
               .HasOne(a => a.Doctor)
               .WithOne(b => b.User)
               .HasForeignKey<Doctor>(e => e.UserID);
        }
    }
}

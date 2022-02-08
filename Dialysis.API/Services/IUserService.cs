using Dialysis.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Dialysis.API.Services
{
    public interface IUserService
    {
        Task<IdentityResult> CreateDoctorAsync(string userName, string password, Doctor doctor);
        Task<IdentityResult> CreatePatientAsync(string userName, string password, Patient patient);
    }
}
using Dialysis.BE.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dialysis.BLL.Users
{
    public interface IUserService
    {
        Task<CreateDoctorResponse> CreateDoctorAsync(CreateDoctorRequest request);
        Task<CreatePatientResponse> CreatePatientAsync(CreatePatientRequest request);
    }
}

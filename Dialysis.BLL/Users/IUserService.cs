using Dialysis.BE.Helpers;
using Dialysis.BE.Users;
using Dialysis.DAL.DTOs;
using Dialysis.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dialysis.BLL.Users
{
    public interface IUserService
    {
        Task<CreateUserResponse> CreateDoctorAsync(CreateDoctorRequest request);
        Task<CreateUserResponse> CreatePatientAsync(CreatePatientRequest request);
        Task<BaseResponse> AssignPatientToDoctorAsync(AssignPatientToDoctorRequest request);
        Task<BaseResponse> UnassignPatientFromDoctorAsync(AssignPatientToDoctorRequest request);
        Task<BaseResponse> DeleteDoctor(int id);
        Task<BaseResponse> DeletePatient(int id);
        Task<BaseResponse> EditDoctor(int id, DoctorDTO doctorDTO);
        Task<BaseResponse> EditPatient(int id, PatientDTO patientDTO);
        Task<GetDoctorsResponse> GetDoctors(bool includePatients, Func<Doctor, bool> filter = null);
        Task<GetPatientsResponse> GetPatients(bool includeDoctors, Func<Patient, bool> filter = null);
        Task<CreateUserResponse> ResetUsersPassword(ResetUsersPasswordRequest request);
        Task<GetUserInfoResponse> GetUserInfo(string userName);
    }
}

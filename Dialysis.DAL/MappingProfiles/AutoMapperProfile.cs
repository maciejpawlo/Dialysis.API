using AutoMapper;
using Dialysis.DAL.DTOs;
using Dialysis.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dialysis.DAL.MappingProfiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<DoctorDTO, Doctor>()
                .ForMember(x => x.DoctorID, opt => opt.Ignore());
            CreateMap<Doctor, DoctorDTO>();
            CreateMap<PatientDTO, Patient>()
                .ForMember(x => x.PatientID, opt => opt.Ignore());
            CreateMap<Patient, PatientDTO>();
            CreateMap<ExaminationDTO, Examination>()
                .ForMember(x => x.ExaminationID, opt => opt.Ignore());
            CreateMap<Examination, ExaminationDTO>();
        }
    }
}

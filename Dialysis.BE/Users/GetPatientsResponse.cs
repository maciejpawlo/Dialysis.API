using Dialysis.BE.Helpers;
using Dialysis.DAL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dialysis.BE.Users
{
    public class GetPatientsResponse : BaseResponse
    {
        public IEnumerable<PatientDTO> Patients { get; set; }
    }
}

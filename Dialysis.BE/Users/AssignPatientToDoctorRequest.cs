using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dialysis.BE.Users
{
    public class AssignPatientToDoctorRequest
    {
        [Required]
        public int PatientID { get; set; }
        [Required]
        public int DoctorID { get; set; }
    }
}

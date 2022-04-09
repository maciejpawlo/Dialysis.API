using Dialysis.DAL.Helpers.Enums;
using PESEL.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dialysis.DAL.DTOs
{
    public class PatientDTO
    {
        public int PatientID { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [Pesel]
        public string PESEL { get; set; }
        public DateTime BirthDate { get; set; }
        [Required]
        public Gender Gender { get; set; }
    }
}

using Dialysis.DAL.Helpers.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dialysis.DAL.Entities
{
    public class Patient
    {
        public int PatientID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PESEL { get; set; }
        public DateTime BirthDate { get; set; }
        public Gender Gender { get; set; }
        public DateTime CreatedAt { get; set; }

        public string UserID { get; set; }
        public User User { get; set; }

        public ICollection<Doctor> Doctors { get; set; }
        public ICollection<Examination> Examinations { get; set; }

        public Patient()
        {
            Doctors = new HashSet<Doctor>();
            Examinations = new HashSet<Examination>();
        }
    }
}

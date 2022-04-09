using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dialysis.DAL.Entities
{
    public class Doctor
    {
        public int DoctorID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PermissionNumber { get; set; }
        public DateTime CreatedAt { get; set; }

        public string UserID { get; set; }
        public User User { get; set; }

        public ICollection<Patient> Patients { get; set; }

        public Doctor()
        {
            Patients = new HashSet<Patient>();
        }
    }
}

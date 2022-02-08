using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dialysis.DAL.Entities
{
    [Table("Doctors")]
    public class Doctor : User
    {
        public long PermissionNumber { get; set; }

        public ICollection<Patient> Patients { get; set; }
    }
}

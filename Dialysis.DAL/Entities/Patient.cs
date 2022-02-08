using Dialysis.DAL.Helpers.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dialysis.DAL.Entities
{
    [Table("Patients")]
    public class Patient : User
    {
        public long PESEL { get; set; }
        public DateTime BirthDate { get; set; }
        public Gender Gender { get; set; }
        public DateTime CreatedAt { get; set; }

        public ICollection<Doctor> Doctors { get; set; }
    }
}

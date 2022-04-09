using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dialysis.DAL.Entities
{
    public class Examination
    {
        public int ExaminationID { get; set; }
        public double Weight { get; set; }
        public double Turbidity { get; set; }
        public string ImageURL { get; set; }
        public DateTime CreatedAt { get; set; }

        public int PatientID { get; set; }
        public Patient Patient { get; set; }
    }
}

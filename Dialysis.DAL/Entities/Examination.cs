using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dialysis.DAL.Entities
{
    public class Examination
    {
        public string ExaminationID { get; set; }
        public int PatientID { get; set; }
        public double Weight { get; set; }
        public double Turbidity { get; set; }
        public string ImageURL { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

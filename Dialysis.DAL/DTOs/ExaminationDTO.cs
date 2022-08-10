using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dialysis.DAL.DTOs
{
    public class ExaminationDTO
    {
        public int ExaminationID { get; set; }
        [Required]
        public double Weight { get; set; }
        [Required]
        public double TurbidityNTU { get; set; }
        [Required]
        public double TurbidityFAU { get; set; }
        public int SystolicPressure { get; set; }
        public int DiastolicPressure { get; set; }
        public string ImageURL { get; set; }
        [Required]
        public int PatientID { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

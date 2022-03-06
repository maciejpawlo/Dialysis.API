using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dialysis.BE.Users
{
    public class CreateDoctorRequest
    {
        public string UserName { get; set; }
        public long PermissionNumber { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
    }
}

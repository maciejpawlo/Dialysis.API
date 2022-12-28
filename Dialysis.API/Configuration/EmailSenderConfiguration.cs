using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dialysis.API.Configuration
{
    public class EmailSenderConfiguration
    {
        public string SmtpProvider { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}

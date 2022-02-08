using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dialysis.BE.Authentication
{
    public class RefreshToken
    {
        public string UserName { get; set; }
        public string Token { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}

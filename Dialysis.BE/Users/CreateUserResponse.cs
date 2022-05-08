using Dialysis.BE.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dialysis.BE.Users
{
    public class CreateUserResponse : BaseResponse
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}

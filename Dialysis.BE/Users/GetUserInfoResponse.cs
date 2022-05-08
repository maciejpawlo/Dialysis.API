using Dialysis.BE.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dialysis.BE.Users
{
    public class GetUserInfoResponse : BaseResponse
    {
        public object Details { get; set; }
    }
}

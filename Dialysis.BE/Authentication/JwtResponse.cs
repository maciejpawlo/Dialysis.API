using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dialysis.BE.Authentication
{
    public class JwtResponse
    {
        public string UserId { get; set; }
        public string AccessToken { get; set; }
        public DAL.Entities.RefreshToken RefreshToken { get; set; }
        public List<string> Errors { get; set; }
    }
}

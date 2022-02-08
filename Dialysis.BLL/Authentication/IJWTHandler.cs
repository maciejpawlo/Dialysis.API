using Dialysis.BE.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Dialysis.BLL.Authentication
{
    public interface IJWTHandler
    {
        JwtResponse GenerateTokens(string userid, Claim[] claims, DateTime date);
        JwtResponse RefreshToken(string userid, string accessToken, string refreshToken, DateTime date);
    }
}

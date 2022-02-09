using Dialysis.BE.Authentication;
using Dialysis.DAL.Entities;
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
        JwtResponse RefreshToken(RefreshToken refreshToken, Claim[] claims, DateTime date);
    }
}

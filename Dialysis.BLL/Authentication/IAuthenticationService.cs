using Dialysis.BE.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dialysis.BLL.Authentication
{
    public interface IAuthenticationService
    {
        Task<AuthenticateResponse> AuthenticateAsync(AuthenticateRequest authenticateRequest);
        Task<AuthenticateResponse> ResfreshTokenAsync(RefreshTokenRequest refreshTokenRequest);
    }
}

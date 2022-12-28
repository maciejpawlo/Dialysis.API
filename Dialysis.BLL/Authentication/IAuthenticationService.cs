using Dialysis.BE.Authentication;
using Dialysis.BE.Helpers;
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
        Task<AuthenticateResponse> RefreshTokenAsync(RefreshTokenRequest refreshTokenRequest);
        Task<BaseResponse> ChangePassword(ChangePasswordRequest resetPasswordRequest, string userName);
        Task<BaseResponse> SetFirstPasswordAsync(SetFirstPasswordRequest request, string userName);
    }
}

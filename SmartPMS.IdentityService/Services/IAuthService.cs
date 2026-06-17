using SmartPMS.IdentityService.DTOs;
using SmartPMS.Shared.Common;

namespace SmartPMS.IdentityService.Services
{
    public interface IAuthService
    {
        Task<ApiResponse<AuthResponseDto>> RegisterAsync(RegisterRequestDto request);

        Task<ApiResponse<AuthResponseDto>> LoginAsync(LoginRequestDto request);
    }
}

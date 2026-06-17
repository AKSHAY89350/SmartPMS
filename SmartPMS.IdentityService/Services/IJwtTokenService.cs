using SmartPMS.IdentityService.Models;

namespace SmartPMS.IdentityService.Services
{
    public interface IJwtTokenService
    {
        string GenerateToken(AppUser user);
    }
}

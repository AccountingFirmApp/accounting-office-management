using AccountingSystem.Application.Intrefaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace AccountingSystem.Infrastructure.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int GetFirmId()
        {
            var firmIdClaim = _httpContextAccessor.HttpContext?.User
                .FindFirst("FirmId")?.Value;

            if (string.IsNullOrEmpty(firmIdClaim))
            {
                throw new UnauthorizedAccessException("FirmId לא נמצא ב-Token");
            }

            return int.Parse(firmIdClaim);
        }

        public int GetUserId()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User
                .FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
            {
                throw new UnauthorizedAccessException("UserId לא נמצא ב-Token");
            }

            return int.Parse(userIdClaim);
        }

        public string GetUserEmail()
        {
            return _httpContextAccessor.HttpContext?.User
                .FindFirst(ClaimTypes.Email)?.Value
                ?? throw new UnauthorizedAccessException("Email לא נמצא ב-Token");
        }

        public string GetUserRole()
        {
            return _httpContextAccessor.HttpContext?.User
                .FindFirst(ClaimTypes.Role)?.Value
                ?? throw new UnauthorizedAccessException("Role לא נמצא ב-Token");
        }
    }
}
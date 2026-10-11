using CompanyName.DDD.Domain.Common.Audit;
using Microsoft.AspNetCore.Http;

namespace CompanyName.DDD.API.Extensions
{
    /// <summary>
    /// Reads the authenticated user (Keycloak "preferred_username" claim) from the current request.
    /// </summary>
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string? UserName => _httpContextAccessor.HttpContext?.User.FindFirst("preferred_username")?.Value;
    }
}

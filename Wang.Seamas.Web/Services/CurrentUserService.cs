using System.Security.Claims;
using Wang.Seamas.Web.Interfaces;

namespace Wang.Seamas.Web.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    
    public int UserId
    {
        get
        {
            var value = _httpContextAccessor.HttpContext?.User?
                .FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(value, out var result) ? result : 0;
        }
    }
    
    public string UserName => _httpContextAccessor.HttpContext?.User?
        .FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;

    public string Email => _httpContextAccessor.HttpContext?.User?
        .FindFirst(ClaimTypes.Email)?.Value ?? string.Empty;

    public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User?
        .Identity?.IsAuthenticated ?? false;

}
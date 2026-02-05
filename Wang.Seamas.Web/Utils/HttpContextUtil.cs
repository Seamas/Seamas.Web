using System.Security.Claims;

namespace Wang.Seamas.Web.Utils;

public class HttpContextUtil
{
    public static int GetCurrentUserId(HttpContext httpContext)
    {
        var value = httpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return int.TryParse(value, out var result) ? result : 0;
    }
}
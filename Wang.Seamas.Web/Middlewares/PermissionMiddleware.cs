using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Wang.Seamas.Web.Attributes;
using Wang.Seamas.Web.Interfaces;

namespace Wang.Seamas.Web.Middlewares;

public class PermissionMiddleware(RequestDelegate next, ILogger<PermissionMiddleware> logger)
{
    
    public async Task InvokeAsync(HttpContext context, IPermissionChecker permissionChecker, IHostEnvironment environment)
    {
        if (context.GetEndpoint()?.Metadata.GetMetadata<AllowAnonymousAttribute>() != null)
        {
            await next(context);
            return;
        }
        
        var url = context.Request.Path.Value?.ToLower();
        var userId = GetCurrentUserId(context);
        // 用户未登录
        if (userId == null)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }
        
        // 用户已登录
        
        
        // TODO: 开发阶段, 短路权限验证
        if (environment.IsDevelopment())
        {
            await next(context);
            return;
        }

        
        // 白名单接口
        if (context.GetEndpoint()?.Metadata.GetMetadata<AllowedListAttribute>() != null)
        {
            await next(context);
            return;
        }
        

        var hasPermission = await permissionChecker.IsUserAllowedToAccessApiAsync(userId.Value, url!);
        if (!hasPermission)
        {
            logger.LogWarning($" 用户 {userId} 没有访问 {url} 的权限 ");
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            return;
        }

        await next(context);
    }


    private int? GetCurrentUserId(HttpContext context)
    {
        var userClaim = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userClaim))
        {
            return null;
        }
        return int.TryParse(userClaim, out var userId) ? userId : null;
    }
}
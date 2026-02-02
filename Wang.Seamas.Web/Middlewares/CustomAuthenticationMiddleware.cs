using System.Security.Claims;
using Microsoft.Extensions.Options;
using Wang.Seamas.Web.Common;
using Wang.Seamas.Web.Common.Exceptions;
using Wang.Seamas.Web.Services;
using Wang.Seamas.Web.Services.Model;

namespace Wang.Seamas.Web.Middlewares;

public class CustomAuthenticationMiddleware(
    RequestDelegate next,
    ITokenService tokenService,
    IOptions<CustomAuthOptions> options,
    IServiceProvider serviceProvider,
    ILogger<CustomAuthenticationMiddleware> logger)
{
    private readonly CustomAuthOptions _options = options.Value;
    private readonly ITokenBlocklistService? _tokenBlocklistService = serviceProvider.GetService<ITokenBlocklistService>();
    private readonly ITokenVersionService? _tokenVersionService = serviceProvider.GetService<ITokenVersionService>();

    public async Task InvokeAsync(HttpContext context)
    {
        // 1. 尝试从请求中获取 token
        var token = ExtractToken(context.Request);

        if (!string.IsNullOrEmpty(token))
        {
            try
            {
                // 验证 token 是否被加入黑名单
                var isBlocked = false;
                if (_tokenBlocklistService != null)
                {
                    isBlocked = await _tokenBlocklistService.IsTokenBlocklistedAsync(token);
                }
                
                if (isBlocked)
                {
                    logger.LogWarning("Token 已被加入黑名单，拒绝访问");
                    await SetUnauthenticated(context);
                    
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return;
                }

                // 2. 验证 token
                var authResult = tokenService.ValidateToken(token);
                
                if (authResult is { IsAuthenticated: true })
                {
                    if (_tokenVersionService != null)
                    {
                        var version = await _tokenVersionService.GetCurrentVersionAsync(authResult.UserId);
                        if (authResult.Version < version)
                        {
                            logger.LogError($"token 版本已经过期");
                            throw new AuthException("token 版本已经过期");

                        }
                    }
                    
                    // 3. 创建 ClaimsPrincipal 并设置到 HttpContext.User
                    var principal = CreateClaimsPrincipal(authResult); 
                    context.User = principal;

                    // 4. 将认证信息也存储到 Items 中供后续中间件使用
                    context.Items["AuthResult"] = authResult;
                    await next(context);
                    return;
                }

                logger.LogWarning("Token 验证失败");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "认证过程中发生错误");
            }
        }
        
        await SetUnauthenticated(context);
        // 5. 调用下一个中间件
        await next(context);
    }

    private string? ExtractToken(HttpRequest request)
    {
        // 尝试从 Header 获取
        if (request.Headers.TryGetValue(_options.TokenHeaderName, out var headerValue))
        {
            var token = headerValue.ToString();
            if (!string.IsNullOrEmpty(token) && token.StartsWith(_options.TokenPrefix))
            {
                return token.Substring(_options.TokenPrefix.Length).Trim();
            }
        }
        
        return null;
    }

    private ClaimsPrincipal CreateClaimsPrincipal(AuthResult authResult)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, authResult.UserId.ToString()),
            new Claim(ClaimTypes.Name, authResult.Username),
            new Claim(ClaimTypes.Email, authResult.Email),
            new Claim("IsAuthenticated", "true"),
            new Claim("AuthenticatedAt", DateTime.UtcNow.ToString("O"))
        };

        // 添加角色声明
        foreach (var role in authResult.Roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        // 添加自定义声明
        foreach (var claim in authResult.Claims)
        {
            claims.Add(new Claim(claim.Key, claim.Value));
        }

        var identity = new ClaimsIdentity(claims, "CustomAuthentication");
        return new ClaimsPrincipal(identity);
    }

    private async Task SetUnauthenticated(HttpContext context)
    {
        // 创建一个匿名用户标识
        var anonymousIdentity = new ClaimsIdentity();
        context.User = new ClaimsPrincipal(anonymousIdentity);
        await Task.CompletedTask;
    }
}
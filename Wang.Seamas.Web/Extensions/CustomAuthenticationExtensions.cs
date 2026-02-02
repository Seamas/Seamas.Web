using Wang.Seamas.Web.Middlewares;
using Wang.Seamas.Web.Services.Model;

namespace Wang.Seamas.Web.Extensions;

public static class CustomAuthenticationExtensions
{
    public static IServiceCollection AddCustomAuthentication(
        this IServiceCollection services,
        Action<CustomAuthOptions> configureOptions)
    {
        // 配置选项
        services.Configure(configureOptions);
        // 注册 Token 服务
        
        // services.AddSingleton<ITokenService, TokenService>();
        return services;
    }

    public static IApplicationBuilder UseCustomAuthentication(this IApplicationBuilder app)
    {
        return app.UseMiddleware<CustomAuthenticationMiddleware>();
    }
}
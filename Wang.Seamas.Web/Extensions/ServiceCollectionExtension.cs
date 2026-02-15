using Wang.Seamas.Web.DTOs;
using Wang.Seamas.Web.Interfaces;
using Wang.Seamas.Web.Services;

namespace Wang.Seamas.Web.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddCustomAuthentication(this IServiceCollection services, Action<CustomAuthOptions> configureOptions)
    {
        services.Configure(configureOptions);
        
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        
        services.AddScoped<ITokenService, TokenService>();

        return services;
    }
}
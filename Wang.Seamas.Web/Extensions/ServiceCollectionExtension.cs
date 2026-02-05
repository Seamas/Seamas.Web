using Wang.Seamas.Web.Services;
using Wang.Seamas.Web.Services.Impl;
using Wang.Seamas.Web.Services.Model;

namespace Wang.Seamas.Web.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddCustomAuthentication(this IServiceCollection services, Action<CustomAuthOptions> configureOptions)
    {
        services.Configure(configureOptions);
        
        return services.AddScoped<ITokenService, TokenService>();
    }
}
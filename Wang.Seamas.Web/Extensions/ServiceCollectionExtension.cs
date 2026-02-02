using Wang.Seamas.Web.Services;
using Wang.Seamas.Web.Services.Impl;

namespace Wang.Seamas.Web.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddCustomAuthentication(this IServiceCollection services)
    {
        return services.AddScoped<ITokenService, TokenService>();
    }
}
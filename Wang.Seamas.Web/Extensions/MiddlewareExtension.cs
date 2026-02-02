using Wang.Seamas.Web.Middlewares;

namespace Wang.Seamas.Web.Extensions;

public static class MiddlewareExtension
{
    
    public static IApplicationBuilder UsePermissionMiddleware(this IApplicationBuilder builder) => builder.UseMiddleware<PermissionMiddleware>();
    
    public static IApplicationBuilder UseGlobalErrorHandlerMiddleware(this IApplicationBuilder builder) => builder.UseMiddleware<GlobalErrorHandlerMiddleware>();
    
    public static IApplicationBuilder UseJsonResultWrapperMiddleware(this IApplicationBuilder builder) => builder.UseMiddleware<JsonResultWrapperMiddleware>();
}
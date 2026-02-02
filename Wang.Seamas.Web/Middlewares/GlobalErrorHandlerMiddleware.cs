using Wang.Seamas.Web.Common.Dtos;
using Wang.Seamas.Web.Common.Exceptions;

namespace Wang.Seamas.Web.Middlewares;

public class GlobalErrorHandlerMiddleware(RequestDelegate next, ILogger<GlobalErrorHandlerMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context); 
            // 被框架自动处理的 http 错误
            if (context.Response.StatusCode != StatusCodes.Status200OK)
            {
                var result = context.Response.StatusCode switch
                {
                    StatusCodes.Status404NotFound => ApiResult.Fail("请求地址错误"),
                    StatusCodes.Status403Forbidden => ApiResult.Fail("未授权访问"),
                    StatusCodes.Status401Unauthorized => ApiResult.Fail("用户未登录"),
                    _ => null
                };
                //context.Response.StatusCode = StatusCodes.Status200OK;
                
                await context.Response.WriteAsJsonAsync(result);
            }
            
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            var result = ex switch
            {
                ValidateException validateException => ApiResult.Fail(validateException.Message),
                BizException bizException => ApiResult.Fail(bizException.Message),
                AuthException authException => ApiResult.Fail(authException.Message, 401),
                _ => ApiResult.Fail("An unexpected error occurred.")
            };
            await context.Response.WriteAsJsonAsync(result);
        }
    }

    
    
}
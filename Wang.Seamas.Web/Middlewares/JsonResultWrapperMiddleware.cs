using System.Text.Json;
using Wang.Seamas.Web.Common;

namespace Wang.Seamas.Web.Middlewares;

public class JsonResultWrapperMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        // 1. 保存原始的响应体流
        var originalBodyStream = context.Response.Body;
        // 2. 创建一个新的内存流来存储响应
        using var responseBody = new MemoryStream();
        // 3. 将响应流替换为我们创建的内存流
        context.Response.Body = responseBody;

        try
        {
            await next(context);
            context.Response.Body = originalBodyStream;
            
            // json 结果自动包装
            if (context.Response.ContentType?.Contains("application/json", StringComparison.InvariantCultureIgnoreCase) ?? false)
            {
                await HandleResponseAsync(context, responseBody);
                return;
            }
            
            await responseBody.CopyToAsync(originalBodyStream);
        }
        catch
        {
            context.Response.Body = originalBodyStream;
            throw;
        }
    }
    
    private async Task HandleResponseAsync(HttpContext context, MemoryStream responseBody)
    {
        responseBody.Seek(0, SeekOrigin.Begin);
        var responseText = await new StreamReader(responseBody).ReadToEndAsync();
        var data = JsonSerializer.Deserialize<object>(responseText);
        var apiResult = ApiResult<object>.Ok(data);
        await context.Response.WriteAsJsonAsync(apiResult);
    }
}
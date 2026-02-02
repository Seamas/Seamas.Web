using Microsoft.AspNetCore.Mvc.Filters;
using Wang.Seamas.Web.Common.Exceptions;

namespace Wang.Seamas.Web.Filters;

public class AsyncModelActionFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.ModelState.IsValid)
        {
            var errors = context.ModelState.Values
                .SelectMany( v => v.Errors)
                .Select(e => e.ErrorMessage)
                .Where(m => !string.IsNullOrWhiteSpace(m))
                .ToArray();

            var message = string.Join("; ", errors);
            throw new ValidateException(message);
        }
        await next();
    }
}
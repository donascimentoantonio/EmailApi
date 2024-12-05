using Microsoft.AspNetCore.Mvc.Filters;

namespace EmailManagement.Api.Filters
{
    public class CustomResultFilter : IAsyncResultFilter

    {

        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = new ValidationFailedResult(context.ModelState);
            }

            await next();
        }

    }
    
}

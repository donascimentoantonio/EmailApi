using EmailManagement.Api.Components;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;

namespace EmailManagement.Api.Middleware
{
    public class ErrorMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try

            {

                await _next(httpContext);

            }

            catch (SecurityTransactionException ex)
            {
                await HandleErrorSecurityAsync(httpContext, ex);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(httpContext, ex);
            }
        }

        private static async Task HandleErrorAsync(HttpContext context, Exception ex)

        {

            var statusCode = StatusCodes.Status500InternalServerError;

            var message = ex.Message ?? ReasonPhrases.GetReasonPhrase(statusCode);

            var errorResponse = new ErrorResponse(statusCode, message);



            context.Response.ContentType = "application/json";

            context.Response.StatusCode = statusCode;

            await context.Response.WriteAsync(JsonConvert.SerializeObject(errorResponse));

        }



        private static async Task HandleErrorSecurityAsync(HttpContext context, SecurityTransactionException ex)
        {
            context.Response.ContentType = "application/json";

            context.Response.StatusCode = ex.StatusCode ?? StatusCodes.Status500InternalServerError;

            await context.Response.WriteAsync(JsonConvert.SerializeObject(JsonConvert.DeserializeObject(ex.Response.ToString())));
        }

    }
}

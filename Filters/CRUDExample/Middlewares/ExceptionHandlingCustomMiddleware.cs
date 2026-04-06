using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace CRUDExample.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class ExceptionHandlingCustomMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingCustomMiddleware> _logger;

        public ExceptionHandlingCustomMiddleware(RequestDelegate next, ILogger<ExceptionHandlingCustomMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    _logger.LogError($"{ex.InnerException.GetType().ToString()} : {ex.InnerException.Message.ToString()}");
                }

                httpContext.Response.StatusCode = 500; // Internal Server Error
                httpContext.Response.WriteAsync($"An unexpected error occurred: {ex.Message}");
            }
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ExceptionHandlingCustomMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionHandlingCustomMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlingCustomMiddleware>();
        }
    }
}

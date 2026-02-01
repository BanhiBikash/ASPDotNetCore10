using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace LoginMiddleware.CustomMiddlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class UseMid
    {
        private readonly RequestDelegate _next;

        public UseMid(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext obj)
        {

            if(obj.Request.Path == "/" && obj.Request.Method == "POST")
            {
                await _next(obj);
            }
            else
            {
                obj.Response.StatusCode = 400;
            }
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseMid(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<UseMid>();
        }
    }
}

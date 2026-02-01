
namespace CustomMiddlewareExtensionMethods.CustomMiddlewares
{
    public class Custom1 : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            await context.Response.WriteAsync("\nWelcome to the implimentation of extension method of custom classes.");
            await context.Response.WriteAsync("\nHello from Custom Middleware 1!\n");
            await context.Response.WriteAsync("\nGoing to Custom Middleware 2!\n");
            await next(context);
            await context.Response.WriteAsync("\nBack to Custom Middleware 1 after Custom Middleware 2!\n");
        }
    }

    public static class Custom1extension
    {
        public static IApplicationBuilder UseCustom1(this IApplicationBuilder app)
        {
            return app.UseMiddleware<Custom1>();
        }
    }
}

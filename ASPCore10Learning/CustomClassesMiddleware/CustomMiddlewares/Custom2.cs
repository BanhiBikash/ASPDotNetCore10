
namespace CustomClassesMiddleware.CustomMiddlewares
{
    public class Custom2 : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            await context.Response.WriteAsync("\nHello from Custom Middleware 2!\n");
            await context.Response.WriteAsync("\nGoing to Terminal Middleware - ShortCircuiting Middleware!\n");
            await next(context);
            await   context.Response.WriteAsync("\nBack to Custom Middleware 2 after Terminal Middleware!\n");
        }
    }
}

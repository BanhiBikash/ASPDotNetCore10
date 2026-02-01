
namespace CustomClassesMiddleware.CustomMiddlewares
{
    public class Custom3 : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            await context.Response.WriteAsync("\nHello from Terminal Middleware 3!\n");
            await context.Response.WriteAsync("\nThis is the end of the pipeline!\n");

            //not calling next(context) to short-circuit the pipeline
        }
    }
}

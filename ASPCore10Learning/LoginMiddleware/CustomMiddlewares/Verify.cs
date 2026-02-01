using Microsoft.Extensions.Primitives;
using System.IO;

namespace LoginMiddleware.CustomMiddlewares
{
    public class Verify : IMiddleware
    {
        bool status=false;

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            StreamReader reader = new StreamReader(context.Request.Body);
            string body = await reader.ReadToEndAsync();

            Dictionary <string,StringValues> data = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(body);

            if (!data.ContainsKey("email"))
            {
                if (!status)
                {
                    context.Response.StatusCode = 400;
                    status= true;
                }

                await context.Response.WriteAsync("\nInvalid Input for Email");
            }

            if(!data.ContainsKey("password"))
            {
                if (!status)
                {
                    context.Response.StatusCode = 400;
                    status = true;
                }

                await context.Response.WriteAsync("\nInvalid Input for password");
            }

            //all checks passed
            if (!status)
            {
                string email = data["email"][0].ToString();
                string password = data["password"][0].ToString();

                if(email == "admin@example.com")
                {
                    if(password == "admin1234")
                    {
                        await next(context);
                    }
                    else
                    {
                        if (!status)
                        {
                            context.Response.StatusCode = 400;
                            status = true;
                        }


                        await context.Response.WriteAsync("Invalid Password");
                    }
                }
                else
                {
                    if (!status)
                    {
                        context.Response.StatusCode = 400;
                        status = true;
                    }

                    await context.Response.WriteAsync("Invalid email");
                }
            }
        }
    }

    public static class VerifyExtensions
    {
        public static IApplicationBuilder UseVerify(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<Verify>();
        }
    }
}

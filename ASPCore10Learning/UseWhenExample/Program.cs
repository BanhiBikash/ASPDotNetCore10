var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.Use(async (obj,next) =>
{
    await obj.Response.WriteAsync("\nThis is the first middleware.");
    await next();
});

//this is the 2nd middleware
app.UseWhen(
    obj => obj.Request.Query.ContainsKey("email"),
    //this is the branch chain
    app =>
    {
        app.Use(async (obj, next) =>
        {
            await obj.Response.WriteAsync("\nThis is branch chain");
            await next();
        });
    }
    );

//this is the terminating middleware
app.Run(async obj =>
{
    await obj.Response.WriteAsync("\nThis is the main branch and we are at the terminating middleware.");
});

app.Run();

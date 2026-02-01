var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();


//example of route parameters with default values
app.Map("user/username/{name=Banhi}",
    async (context) =>
    {
        string? name = context.Request.RouteValues["name"]?.ToString();
        await context.Response.WriteAsync($"\nHello {name}");
    });

//example of route parameters with optional values
app.Map("user/userid/{id?}",
    async (context) =>
    {
        if (context.Request.RouteValues.ContainsKey("id"))
        {
            await context.Response.WriteAsync($"\nId is {context.Request.RouteValues["id"]}.");
        }
        else
        {
            await context.Response.WriteAsync("\nno Id has been provided.");
        }
    });

app.MapFallback(async (context) =>
{
    await context.Response.WriteAsync("\nNo routing described for this specific path.");
});
app.Run();

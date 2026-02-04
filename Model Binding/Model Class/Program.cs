var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var app = builder.Build();
app.MapControllers();

app.MapFallback( async (Context) =>
{
    Context.Response.WriteAsync("\nPlease enter a valid URL.");
});

app.Run();

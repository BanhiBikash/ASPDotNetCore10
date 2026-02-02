var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var app = builder.Build();

app.UseStaticFiles();

app.MapControllers();

app.MapFallback(async (context) =>
{
    await context.Response.WriteAsync("\nThis is fallback.");
});
app.Run();

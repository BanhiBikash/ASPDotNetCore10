var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

var app = builder.Build();
app.UseStaticFiles();
app.MapControllers();

app.MapFallback(async (context) => await context.Response.WriteAsync( "Hello World! From Fallback."));

app.Run();

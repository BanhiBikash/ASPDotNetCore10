var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
var app = builder.Build();
app.MapControllers();
app.UseStaticFiles();
app.MapFallback(async (context) => await context.Response.WriteAsync("Hello World!"));

app.Run();

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

var app = builder.Build();
app.MapControllers();
app.UseStaticFiles();
app.MapFallback(async (c) =>await c.Response.WriteAsync( "Hello World! From Fallback"));

app.Run();

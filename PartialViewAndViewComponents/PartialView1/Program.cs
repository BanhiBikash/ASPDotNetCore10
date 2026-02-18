var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
var app = builder.Build();
app.MapControllers();
app.UseStaticFiles();
app.MapFallback(async (obj) =>await obj.Response.WriteAsync( "Hello World! From fallback."));

app.Run();
